using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class SystemRole
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // Employee, TeamLead, HR, CTO, CEO

        [MaxLength(500)]
        public string? Description { get; set; }

        // Permission level (za buduće permission logic)
        public int PermissionLevel { get; set; } // 1=Employee, 2=TeamLead, 3=HR, 4=CTO, 5=CEO

        public bool IsActive { get; set; } = true;

        // Users with this system role
        public ICollection<User> Users { get; set; } = new List<User>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}