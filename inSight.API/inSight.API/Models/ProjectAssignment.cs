using System;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class ProjectAssignment
    {
        [Key]
        public Guid Id { get; set; }

        // Ko je dodeljen na projekat
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Kom projektu je dodeljen
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        // Uloga na projektu
        public ProjectRole Role { get; set; }

        // Da li vodi projekat?
        public bool IsProjectLead { get; set; } = false;

        // Period angažovanja
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}