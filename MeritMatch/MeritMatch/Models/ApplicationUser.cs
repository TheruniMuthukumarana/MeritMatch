using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public ICollection<Project> StudentProjects { get; set; }
    public ICollection<Project> SupervisedProjects { get; set; }
    public ICollection<SupervisorExpertise> Expertises { get; set; }
}
