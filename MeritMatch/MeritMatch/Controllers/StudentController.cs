using MeritMatch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// ...rest of your code...


[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly ProjectService _projectService;
    public StudentController(ProjectService projectService) => _projectService = projectService;

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var projects = await _projectService.GetProjectsByStudentAsync(userId);
        return View(projects);
    }

    // Create, Edit, Delete actions...
}
