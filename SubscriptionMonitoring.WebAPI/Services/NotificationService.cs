using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.MVC.Controllers;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;

namespace SubscriptionMonitoring.WebAPI.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;
        private readonly ProjectService _projectService;

        public NotificationService(AppDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            if (notification.NotificationSender=="Admin" && _context.Users.Any(i => i.UserId == notification.UserId)){
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return notification;
            }
            else if (notification.NotificationSender == "company" && _context.Users.Any(i => i.UserId == notification.UserId) && _context.Projects.Any(i => i.ProjectId == notification.UserId))
            {
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return notification;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Notification>> GetCompanyNotifications(string companyid)
        {
            List<Project> AllProjects = new List<Project>();
            AllProjects = _projectService.GetAllProjects(companyid);
            List<Notification> Allnotifications = new List<Notification>();
            foreach (var project in AllProjects)
            {
                var notifications = await _context.Notifications
                                           .Where(u => u.UserId == project.ProjectId)
                                           .ToListAsync();
                Allnotifications.AddRange(notifications);
            }
            
            return Allnotifications;

        }

        public List<Notification> GetTop3Notifications(string userId)
        {
            return _context.Notifications.Where(u=>u.UserId == userId).OrderByDescending(n => n.NotificationSentDate)
                                         .Take(3)
                                         .ToList();
        }






    }
}
