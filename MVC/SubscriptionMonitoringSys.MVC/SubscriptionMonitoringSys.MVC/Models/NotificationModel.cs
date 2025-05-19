using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoringSys.MVC.Models
{
    public class NotificationModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string NotificationMessage { get; set; }

        [Required]
        public string Type { get; set; }
        [Required]

        public string NotificationSender { get; set; }

        public DateTime NotificationSentDate { get; set; } = DateTime.Now;
    }
}
