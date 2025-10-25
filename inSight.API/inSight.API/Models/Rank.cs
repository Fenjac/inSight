using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inSight.API.Models
{
    public class Rank
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty; // P1, J1, M1, S1, E1, etc.

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Praktikant 1, Junior 1, Medior 1, etc.

        [MaxLength(500)]
        public string? Description { get; set; }

        // Salary range
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxSalary { get; set; }

        // Order for sorting
        public int OrderIndex { get; set; }

        public bool IsActive { get; set; } = true;

        // Users with this rank
        public ICollection<User> Users { get; set; } = new List<User>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}