using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeritMatch.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public List<UserViewModel> Users { get; set; } = new();
        public List<ApplicationUser> Students { get; set; } = new();
        public List<ApplicationUser> Supervisors { get; set; } = new();
        public List<ResearchArea> ResearchAreas { get; set; } = new();
        public List<Project> Projects { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            Users = new List<UserViewModel>();
            Students = new List<ApplicationUser>();
            Supervisors = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "";
                Users.Add(new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = role,
                    ResearchArea = user.ResearchArea
                });
                if (role == "Student")
                    Students.Add(user);
                if (role == "Supervisor")
                    Supervisors.Add(user);
            }

            ResearchAreas = await _dbContext.ResearchAreas.ToListAsync();

            Projects = await _dbContext.Projects
                .Include(p => p.ResearchArea)
                .Include(p => p.Student)
                .Include(p => p.Supervisor)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                // Remove related data if needed (projects, expertises, etc.)
                var projects = await _dbContext.Projects.Where(p => p.StudentId == id || p.SupervisorId == id).ToListAsync();
                _dbContext.Projects.RemoveRange(projects);

                var expertises = await _dbContext.SupervisorExpertises.Where(se => se.SupervisorId == id).ToListAsync();
                _dbContext.SupervisorExpertises.RemoveRange(expertises);

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteProjectAsync(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddUserAsync(string AddUserName, string AddUserEmail, string AddUserPassword, string AddUserRole, string AddUserResearchArea)
        {
            var user = new ApplicationUser
            {
                UserName = AddUserEmail,
                Email = AddUserEmail,
                Name = AddUserName,
                ResearchArea = AddUserResearchArea
            };
            var result = await _userManager.CreateAsync(user, AddUserPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AddUserRole);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddProjectAsync(string AddProjectTitle, string AddProjectStudentId, string AddProjectSupervisorId, string AddProjectStatus, int AddProjectResearchAreaId, string AddProjectTechStack, string AddProjectAbstract)
        {
            var project = new Project
            {
                Title = AddProjectTitle,
                StudentId = AddProjectStudentId,
                SupervisorId = string.IsNullOrEmpty(AddProjectSupervisorId) ? null : AddProjectSupervisorId,
                Status = Enum.TryParse<ProjectStatus>(AddProjectStatus, out var status) ? status : ProjectStatus.Pending,
                ResearchAreaId = AddProjectResearchAreaId,
                TechStack = AddProjectTechStack,
                Abstract = AddProjectAbstract
            };
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
            return RedirectToPage();
        }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string ResearchArea { get; set; }
        }
    }
}

