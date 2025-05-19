using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;

namespace SubscriptionMonitoring.WebAPI.Services
{
    public class FeedbackService
    {
        private readonly AppDbContext _context;
        private readonly NotificationService _notificationService;

        public FeedbackService(AppDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            var notification = new Notification()
            {
                NotificationSender = "Admin",
                NotificationMessage = "Feedback Added!",
                NotificationSentDate = DateTime.Today,
                UserId = feedback.UserId,
                Type = "Feedback Added"
            };
            Notification sendNotification = await _notificationService.CreateNotificationAsync(notification);
            return feedback;
        }
        public async Task<Feedback> DeleteFeedbackAsync(Feedback feedback)
        {

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            var notification = new Notification()
            {
                NotificationSender = "Admin",
                NotificationMessage = "Feedback Deleted!",
                NotificationSentDate = DateTime.Today,
                UserId = feedback.UserId,
                Type = "Feedback Deleted"
            };
            Notification sendNotification = await _notificationService.CreateNotificationAsync(notification);
            return feedback;

        }
    }
}
