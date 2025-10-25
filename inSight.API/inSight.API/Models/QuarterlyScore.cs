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

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public Guid QuarterId { get; set; }

        [ForeignKey(nameof(QuarterId))]
        public Quarter Quarter { get; set; } = null!;

        /// <summary>
        /// Base score from evaluations for this quarter
        /// For regular employees: score from their evaluation
        /// For leaders: average score from evaluations they received
        /// </summary>
        [Required]
        public int BaseScore { get; set; }

        /// <summary>
        /// Average score for leaders (calculated from evaluations where they were evaluated)
        /// Null for non-leaders
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? LeaderAverageScore { get; set; }

        /// <summary>
        /// Bonus points added by management for this quarter
        /// </summary>
        public int ManagementBonusScore { get; set; } = 0;

        /// <summary>
        /// Reason/explanation for management bonus
        /// </summary>
        [MaxLength(500)]
        public string? ManagementBonusReason { get; set; }

        /// <summary>
        /// Total score for this quarter (BaseScore + ManagementBonusScore)
        /// </summary>
        [Required]
        public int TotalScore { get; set; }

        /// <summary>
        /// Optional: Foreign key to the main evaluation if this score is derived from one
        /// </summary>
        public Guid? EvaluationId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Calculate total score from base and bonus
        /// </summary>
        public void CalculateTotalScore()
        {
            TotalScore = BaseScore + ManagementBonusScore;
        }
    }
}