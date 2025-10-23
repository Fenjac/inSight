using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Developer, QA, Application Engineer

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Ranks for this role
        public ICollection<Rank> Ranks { get; set; } = new List<Rank>();

        // Users with this role
        public ICollection<User> Users { get; set; } = new List<User>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}