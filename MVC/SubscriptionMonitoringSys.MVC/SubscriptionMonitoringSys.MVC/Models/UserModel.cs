namespace SubscriptionMonitoringSys.MVC.Models
{
    public class UserModel
    {

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; } 
        public string UserPasswordHash { get; set; }
        public DateTime UserCreatedAt { get; set; } = DateTime.Now;
    }
}
