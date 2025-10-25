using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inSight.API.Models
{
    /// <summary>
    /// Defines score thresholds for each rank per role
    /// Different roles (Developer, QA Engineer, Application Engineer) have different thresholds
    /// </summary>
    public class RankScoreThreshold
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid RankId { get; set; }

        [ForeignKey(nameof(RankId))]
        public Rank Rank { get; set; } = null!;

        [Required]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Minimum score required for this rank
        /// </summary>
        [Required]
        public int MinScore { get; set; }

        /// <summary>
        /// Maximum score for this rank (null for highest rank means no upper limit)
        /// </summary>
        public int? MaxScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Check if a given score falls within this threshold range
        /// </summary>
        public bool IsScoreInRange(int score)
        {
            if (MaxScore.HasValue)
            {
                return score >= MinScore && score <= MaxScore.Value;
            }
            return score >= MinScore;
        }
    }
}
