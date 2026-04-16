using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeritMatch.Services;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    // ManageUsers, ManageResearchAreas, ViewAllMatches actions...
}
