public class SupervisorExpertise
{
    public int Id { get; set; }
    public string SupervisorId { get; set; }
    public ApplicationUser Supervisor { get; set; }
    public int ResearchAreaId { get; set; }
    public ResearchArea ResearchArea { get; set; }
}
