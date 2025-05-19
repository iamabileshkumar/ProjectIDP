using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoring.WebAPI.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }
        public string UserId { get; set; }
        public User user { get; set; }
        public string SubscriptionName { get; set; } 
        public double SubscriptionPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        
    }
}
