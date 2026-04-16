using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ResearchArea> ResearchAreas { get; set; }
    public DbSet<SupervisorExpertise> SupervisorExpertises { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Student)
            .WithMany(u => u.StudentProjects)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Supervisor)
            .WithMany(u => u.SupervisedProjects)
            .HasForeignKey(p => p.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SupervisorExpertise>()
            .HasOne(se => se.Supervisor)
            .WithMany(u => u.Expertises)
            .HasForeignKey(se => se.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SupervisorExpertise>()
            .HasOne(se => se.ResearchArea)
            .WithMany(ra => ra.SupervisorExpertises)
            .HasForeignKey(se => se.ResearchAreaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

