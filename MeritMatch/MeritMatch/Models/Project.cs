public enum ProjectStatus { Pending, UnderReview, Matched }

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string TechStack { get; set; }
    public ProjectStatus Status { get; set; }
    public int ResearchAreaId { get; set; }
    public ResearchArea ResearchArea { get; set; }
    public string StudentId { get; set; }
    public ApplicationUser Student { get; set; }
    public string? SupervisorId { get; set; }
    public ApplicationUser Supervisor { get; set; }
}
