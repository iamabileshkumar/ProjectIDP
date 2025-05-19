namespace SubscriptionMonitoringSys.MVC.Models
{
    public class CombinedViewModel
    {
        public ReportModel ReportFilter { get; set; }
        public IEnumerable<SubscriptionModel> Subscriptions { get; set; }
        public IEnumerable<NotificationModel> Notifications { get; set; }

        public int NumberOfProjects { get; set; }
        public int NumberOfUsers { get; set; }

        public CombinedViewModel() { }

        public CombinedViewModel(ReportModel reportFilter, List<NotificationModel> notifications, int numberOfProjects,int numberOfUsers)
        {
            ReportFilter = reportFilter;
            Notifications = notifications;
            NumberOfProjects = numberOfProjects;
            NumberOfUsers = numberOfUsers;
            Subscriptions = new List<SubscriptionModel>();
        }
    }
}
