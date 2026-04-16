using MeritMatch.Data;
using MeritMatch.Models;
using Microsoft.EntityFrameworkCore;

namespace MeritMatch.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Example: Get all projects
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.ResearchArea)
                .ToListAsync();
        }

        // Example: Create a new project
        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        internal async Task<string?> GetProjectsByStudentAsync(string? userId)
        {
            throw new NotImplementedException();
        }

        internal async Task<string?> GetAnonymousProjectsForSupervisorAsync(string? supervisorId)
        {
            throw new NotImplementedException();
        }

        // Add more methods as needed (Update, Delete, Match, etc.)
    }
}
