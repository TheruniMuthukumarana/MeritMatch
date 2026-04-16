using MeritMatch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = "Supervisor")]
public class SupervisorController : Controller
{
    private readonly ProjectService _projectService;
    public SupervisorController(ProjectService projectService) => _projectService = projectService;

    public async Task<IActionResult> AnonymousProjects()
    {
        var supervisorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var projects = await _projectService.GetAnonymousProjectsForSupervisorAsync(supervisorId);
        return View(projects);
    }

    // ExpressInterest, ConfirmMatch actions...
}
