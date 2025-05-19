namespace SubscriptionMonitoring.WebAPI.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string UserId { get; set; }
        public User user { get; set; }
        public string FeedbackText { get; set; }
        public int FeedbackRating { get; set; }
        public DateTime FeedbackCreatedAt { get; set; } = DateTime.Now;
           
    }
}
