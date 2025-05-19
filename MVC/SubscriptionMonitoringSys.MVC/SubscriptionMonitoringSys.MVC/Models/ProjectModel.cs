using System.ComponentModel.DataAnnotations;

namespace SubscriptionMonitoringSys.MVC.Models
{
    public class ProjectModel
    {
        [Required]
        public string ProjectId { get; set; }

        [Required]
        public string UserId { get; set; } //Here User is Company

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProjectManagerName { get; set; }

        [Required]
        public long Budget { get; set; }

        [Required]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        public DateTime ProjectEndDate { get; set; }

        public string Projectstatus { get; set; } = string.Empty;
    }
}
