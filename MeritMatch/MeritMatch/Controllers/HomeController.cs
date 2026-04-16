using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MeritMatch.Models;
using MeritMatch.Services;

namespace MeritMatch.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProjectService _projectService;

    public HomeController(ILogger<HomeController> logger, ProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
