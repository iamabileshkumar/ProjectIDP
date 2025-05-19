using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;

namespace SubscriptionMonitoring.WebAPI.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;
        

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }
        

        public bool ProjectExists(string id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        public List<Project> GetAllProjects(string id)
        {
            return _context.Projects.Where(i => i.UserId == id).ToList();
        }

    }
}
