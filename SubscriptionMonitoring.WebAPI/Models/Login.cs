using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoring.WebAPI.Models
{
    public class Login

    {
        [Required]
        public string LoginUserId { get; set; }
        
        [Required]
        public string LoginUserPassword { get; set; } 

    }
}
