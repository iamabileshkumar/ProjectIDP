using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;
using SubscriptionMonitoring.WebAPI.Services;

namespace SubscriptionMonitoring.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("user/{userId}")]
        public IActionResult GetUserReport(string userId, [FromBody] ReportFilter filter)
        {
            var report = _context.Subscriptions
                .Where(e => e.UserId == userId &&
                            (!filter.StartDate.HasValue || e.SubscriptionStartDate >= filter.StartDate) &&
                            (!filter.EndDate.HasValue || e.SubscriptionEndDate <= filter.EndDate) &&
                            (string.IsNullOrEmpty(filter.Category) || e.CategoryName == filter.Category) &&
                            (!filter.MinPrice.HasValue || e.SubscriptionPrice >= filter.MinPrice) &&
                            (!filter.MaxPrice.HasValue || e.SubscriptionPrice <= filter.MaxPrice) &&
                            (string.IsNullOrEmpty(filter.Status) || e.Status == filter.Status))
                .ToList();

            return Ok(report);
        }

        [HttpPost("company/user/{userId}")]
        public IActionResult GetCompanyUserReport(string userId, [FromBody] ReportFilter filter)
        {
            
            
            var report = _context.Subscriptions
                .Where(e => e.UserId == userId && _context.Projects.Any(p=>p.ProjectId == e.UserId)&&
                            (!filter.StartDate.HasValue || e.SubscriptionStartDate >= filter.StartDate) &&
                            (!filter.EndDate.HasValue || e.SubscriptionEndDate <= filter.EndDate) &&
                            (string.IsNullOrEmpty(filter.Category) || e.CategoryName == filter.Category) &&
                            (!filter.MinPrice.HasValue || e.SubscriptionPrice >= filter.MinPrice) &&
                            (!filter.MaxPrice.HasValue || e.SubscriptionPrice <= filter.MaxPrice) &&
                            (string.IsNullOrEmpty(filter.Status) || e.Status == filter.Status))
                .ToList();

            return Ok(report);
        }

        [HttpPost("company/{userId}")]
        public IActionResult GetCompanyAllReport(string userId, [FromBody] ReportFilter filter)
        {
            var AllProjects = _context.Projects.ToList();

            var report = _context.Subscriptions
                .Where(e => _context.Projects.Any(p => p.ProjectId == e.UserId && p.UserId == userId) &&
                            (!filter.StartDate.HasValue || e.SubscriptionStartDate >= filter.StartDate) &&
                            (!filter.EndDate.HasValue || e.SubscriptionEndDate <= filter.EndDate) &&
                            (string.IsNullOrEmpty(filter.Category) || e.CategoryName == filter.Category) &&
                            (!filter.MinPrice.HasValue || e.SubscriptionPrice >= filter.MinPrice) &&
                            (!filter.MaxPrice.HasValue || e.SubscriptionPrice <= filter.MaxPrice) &&
                            (string.IsNullOrEmpty(filter.Status) || e.Status == filter.Status))
                .ToList();

            return Ok(report);
        }

        [HttpPost("all")]
        public IActionResult GetAllUsersReport([FromBody] ReportFilter filter)
        {
            var report = _context.Subscriptions
                .Where(e => (!filter.StartDate.HasValue || e.SubscriptionStartDate >= filter.StartDate) &&
                            (!filter.EndDate.HasValue || e.SubscriptionEndDate <= filter.EndDate) &&
                            (string.IsNullOrEmpty(filter.Category) || e.CategoryName == filter.Category) &&
                            (!filter.MinPrice.HasValue || e.SubscriptionPrice >= filter.MinPrice) &&
                            (!filter.MaxPrice.HasValue || e.SubscriptionPrice <= filter.MaxPrice) &&
                            (string.IsNullOrEmpty(filter.Status) || e.Status == filter.Status))
                .ToList();

            return Ok(report);
        }
    }
}
