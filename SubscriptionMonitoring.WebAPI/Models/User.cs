using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionMonitoring.WebAPI.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPasswordHash { get; set; }
        public string Role { get; set; } = "User";
        public DateTime UserCreatedAt { get; set; } = DateTime.Now;
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }

    }
}
