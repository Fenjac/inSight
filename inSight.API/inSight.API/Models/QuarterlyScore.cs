using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inSight.API.Models
{
    public class QuarterlyScore
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public Guid QuarterId { get; set; }
        public Quarter Quarter { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal EvaluationScore { get; set; } // Bodovi iz upitnika

        [Column(TypeName = "decimal(5,2)")]
        public decimal ManagementBonusPoints { get; set; } = 0; // Bonus od managementa

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalScore { get; set; } // Ukupno

        [MaxLength(10)]
        public string? AssignedRank { get; set; } // Rang dodeljen za taj kvartal

        public string? ManagementComment { get; set; } // Komentar od managementa uz bonus

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}