using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoringSys.MVC.Models
{
    public class FeedbackModel
    {
        
        public int FeedbackId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FeedbackText { get; set; }
        [Required]
        public int FeedbackRating { get; set; }

        public DateTime FeedbackCreatedAt { get; set; } = DateTime.Now;
    }
}
