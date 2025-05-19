using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoring.WebAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } 
        public DateTime CategoryCreatedAt { get; set; } = DateTime.Now;
        public ICollection<Subscription> SubscriptionListByCategory { get; set; }
    }
}
