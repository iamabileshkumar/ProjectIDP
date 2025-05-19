using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;

namespace SubscriptionMonitoring.WebAPI.Services
{
    public class SubscriptionStatusService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        public SubscriptionStatusService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Adjust the interval as needed

            return Task.CompletedTask;
        }
        private async void ExecuteAsync(object state)
        {
            
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var subscriptions = context.Subscriptions.ToList();

                    foreach (var subscription in subscriptions)
                    {
                        subscription.Status = GetStatus(subscription.SubscriptionEndDate);
                    using (var service = _scopeFactory.CreateScope())
                    {
                        var notificationService = service.ServiceProvider.GetRequiredService<NotificationService>();
                        if (subscription.Status == "Expired Soon")
                        {
                            var notification = new Notification()
                            {
                                NotificationSender = "Admin",
                                NotificationMessage = $"Your {subscription.SubscriptionName} Expires Soon!",
                                NotificationSentDate = DateTime.Today,
                                UserId = subscription.UserId,
                                Type = "Alert"
                            };
                            Notification sendNotification = await notificationService.CreateNotificationAsync(notification);
                        }
                        else if (subscription.Status == "Expired")
                        {
                            var notification = new Notification()
                            {
                                NotificationSender = "Admin",
                                NotificationMessage = $"Your {subscription.SubscriptionName} Expired!",
                                NotificationSentDate = DateTime.Today,
                                UserId = subscription.UserId,
                                Type = "Alert"
                            };
                            Notification sendNotification = await notificationService.CreateNotificationAsync(notification);
                        }
                    }
                    
                }

                    await context.SaveChangesAsync();
                }
    
        }
        private string GetStatus(DateTime expiryDate)
        {
            var currentDate = DateTime.UtcNow;
            if (expiryDate < currentDate)
            {
                return "Expired";
            }
            else if ((expiryDate - currentDate).TotalDays <= 2)
            {
                return "Expired Soon";
            }
            else
            {
                return "Active";
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

