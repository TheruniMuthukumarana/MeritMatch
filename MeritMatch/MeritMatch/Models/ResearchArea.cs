public class ResearchArea
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Project> Projects { get; set; }
    public ICollection<SupervisorExpertise> SupervisorExpertises { get; set; }
}
