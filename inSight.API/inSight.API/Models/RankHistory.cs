using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class RankHistory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string OldRank { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string NewRank { get; set; } = string.Empty;

        [Required]
        public Guid QuarterId { get; set; }
        public Quarter Quarter { get; set; } = null!;

        [Required]
        public Guid ChangedById { get; set; }
        public User ChangedBy { get; set; } = null!;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}