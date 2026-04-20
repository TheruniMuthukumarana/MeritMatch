using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize(Roles = "Student")]
public class HomeModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public HomeModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public List<Project> Proposals { get; set; }
    public string ErrorMessage { get; set; }

    [BindProperty]
    public string Title { get; set; }
    [BindProperty]
    public string TechStack { get; set; }
    [BindProperty]
    public string Abstract { get; set; }

    // For editing
    [BindProperty]
    public int EditId { get; set; }
    [BindProperty]
    public string EditTitle { get; set; }
    [BindProperty]
    public string EditTechStack { get; set; }
    [BindProperty]
    public string EditAbstract { get; set; }

    // For deleting
    [BindProperty]
    public int DeleteId { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        Proposals = await _dbContext.Projects
            .Include(p => p.ResearchArea)
            .Include(p => p.Supervisor) // This ensures Supervisor is loaded
            .Where(p => p.StudentId == user.Id)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostCreateProposalAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        // Find the research area entity that matches the student's selected area (case-insensitive)
        var userResearchArea = await _dbContext.ResearchAreas
            .FirstOrDefaultAsync(r => r.Name.ToLower() == user.ResearchArea.ToLower());

        if (userResearchArea == null)
        {
            ErrorMessage = "Your research area is not available. Please contact admin.";
            await OnGetAsync();
            return Page();
        }

        var project = new Project
        {
            Title = Title,
            TechStack = TechStack,
            Abstract = Abstract,
            StudentId = user.Id,
            Status = ProjectStatus.Pending, // Set your default status
            ResearchAreaId = userResearchArea.Id
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(); // Refresh to show the new proposal
    }

    public async Task<IActionResult> OnPostEditProposalAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        var proposal = await _dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == EditId && p.StudentId == user.Id);

        if (proposal == null)
        {
            ErrorMessage = "Proposal not found or you do not have permission to edit it.";
            await OnGetAsync();
            return Page();
        }

        proposal.Title = EditTitle;
        proposal.TechStack = EditTechStack;
        proposal.Abstract = EditAbstract;

        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteProposalAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        var proposal = await _dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == DeleteId && p.StudentId == user.Id);

        if (proposal == null)
        {
            ErrorMessage = "Proposal not found or you do not have permission to delete it.";
            await OnGetAsync();
            return Page();
        }

        _dbContext.Projects.Remove(proposal);
        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }
}

