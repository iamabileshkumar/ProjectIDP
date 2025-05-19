using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoring.WebAPI.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public string NotificationSender { get; set; }
        public string UserId { get; set; }
        public User user { get; set; }
        public string NotificationMessage { get; set; }
        public string Type { get; set; }
        public DateTime NotificationSentDate { get; set; } = DateTime.Now;
    }
}
