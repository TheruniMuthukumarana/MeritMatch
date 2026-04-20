using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Supervisor")]
public class SupervisorHomeModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public SupervisorHomeModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public List<Project> Proposals { get; set; } = new();
    [BindProperty]
    public List<int> RevealedStudentIds { get; set; } = new();

    public async Task OnGetAsync()
    {
        var supervisor = await _userManager.GetUserAsync(User);

        // Get the supervisor's research area (assuming it's a string property)
        var supervisorResearchArea = supervisor.ResearchArea?.Trim().ToLower();

        // Show all proposals in the same research area (assigned or not)
        Proposals = await _dbContext.Projects
            .Include(p => p.ResearchArea)
            .Include(p => p.Supervisor)
            .Include(p => p.Student)
            .Where(p => p.ResearchArea.Name.ToLower() == supervisorResearchArea)
            .ToListAsync();

        // Keep revealed IDs in TempData if any
        if (TempData.ContainsKey("RevealedStudentIds"))
        {
            RevealedStudentIds = TempData["RevealedStudentIds"].ToString()
                .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
            TempData.Keep("RevealedStudentIds");
        }
    }

    public async Task<IActionResult> OnPostApproveAsync(int id)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Supervisor)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project != null && project.Status == ProjectStatus.Pending)
        {
            project.Status = ProjectStatus.Approved;

            // Assign the current supervisor
            var supervisor = await _userManager.GetUserAsync(User);
            if (supervisor != null)
            {
                project.SupervisorId = supervisor.Id;
                project.Supervisor = supervisor;
            }

            await _dbContext.SaveChangesAsync();
        }

        // Track revealed student IDs in TempData
        List<int> revealed = new();
        if (TempData.ContainsKey("RevealedStudentIds"))
        {
            revealed = TempData["RevealedStudentIds"].ToString()
                .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
        if (!revealed.Contains(id))
            revealed.Add(id);

        TempData["RevealedStudentIds"] = string.Join(",", revealed);

        return RedirectToPage();
    }
}
