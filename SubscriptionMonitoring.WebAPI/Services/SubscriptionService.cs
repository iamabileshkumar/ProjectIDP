using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;

namespace SubscriptionMonitoring.WebAPI.Services
{
    public class SubscriptionService
    {
        private readonly AppDbContext _context;
        private readonly CategoryService _categoryService;
        private readonly NotificationService _notificationService;


        public SubscriptionService(AppDbContext context, CategoryService categoryService,NotificationService notificationService)
        {
            _context = context;
            _categoryService = categoryService;
            _notificationService = notificationService;
            
        }
        public async Task<Subscription> CreateSubscriptionAsync(Subscription model)
        {
            
            int categoryId = _categoryService.CategoryExists(model.CategoryName);
            if (categoryId == 0)
            {
                var category = await _categoryService.GetOrCreateCategoryAsync(model.CategoryName);
                

                model.CategoryId = category.CategoryId;
                
            }
            else
                model.CategoryId = categoryId;
            _context.Subscriptions.Add(model);
            await _context.SaveChangesAsync();
            var notification = new Notification()
            {
                NotificationSender = "Admin",
                NotificationMessage = "New Subscription Added!",
                NotificationSentDate = DateTime.Today,
                UserId = model.UserId,
                Type="Subscription Added"
            };
            Notification sendNotification = await _notificationService.CreateNotificationAsync(notification);
            return model;

            
        }

        
        public bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

    }
}
