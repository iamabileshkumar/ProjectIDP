namespace SubscriptionMonitoring.WebAPI.Models
{
    public class Project
    {
        public string ProjectId { get; set; }
        public string UserId { get; set; } //Here User is Company
        public User user { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManagerName { get; set; }
        public long Budget { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string Projectstatus { get; set; } = string.Empty;

    }
}
