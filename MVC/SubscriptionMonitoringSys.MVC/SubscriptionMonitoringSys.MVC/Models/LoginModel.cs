using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoringSys.MVC.Models
{
    public class LoginModel
    {
        [Required]
        public string LoginUserId { get; set; }

        [Required]
        public string LoginUserPassword { get; set; } = string.Empty;
    }
}
