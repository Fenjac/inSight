using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inSight.API.Models
{
    public class RankConfiguration
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Link to Rank
        public Guid RankId { get; set; }
        public Rank Rank { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MinPoints { get; set; } // Minimum bodova za ovaj rang

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MaxPoints { get; set; } // Maximum bodova za ovaj rang

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; } // Plata za ovaj rang

        [Required]
        public DateTime EffectiveFrom { get; set; } // Od kada važi ova konfiguracija

        public DateTime? EffectiveTo { get; set; } // Do kada važi (null = trenutno aktivna)

        public bool IsActive { get; set; } = true;

        public string? Notes { get; set; } // Beleške (npr. "Inflacija 2025")

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}