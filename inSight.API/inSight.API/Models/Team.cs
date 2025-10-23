using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // Team Lead
        public Guid? TeamLeadId { get; set; }
        public User? TeamLead { get; set; }

        // Members
        public ICollection<User> Members { get; set; } = new List<User>();

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}