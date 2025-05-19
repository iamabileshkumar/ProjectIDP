namespace SubscriptionMonitoringSys.MVC.Models
{
    public class ReportModel
    {
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Category { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string Status { get; set; }
    }
}
