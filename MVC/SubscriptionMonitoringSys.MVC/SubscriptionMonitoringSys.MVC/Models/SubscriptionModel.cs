using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoringSys.MVC.Models
{
    public class SubscriptionModel
    {
        //[Key]
        public int SubscriptionId { get; set; }
        public string UserId { get; set; }

        public UserModel user { get; set; }
        [Required]
        public string SubscriptionName { get; set; }
        [Required]
        public double SubscriptionPrice { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }
        [Required]
        public DateTime SubscriptionStartDate { get; set; }
        [Required]
        public DateTime SubscriptionEndDate { get; set; } 

        public string Status { get; set; }
    }
}
