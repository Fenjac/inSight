using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class Quarter
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int Year { get; set; }

        [Required]
        [Range(1, 4)]
        public int QuarterNumber { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = false;

        public bool IsLocked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        public ICollection<LeaderJournal> LeaderJournals { get; set; } = new List<LeaderJournal>();
        public ICollection<RankHistory> RankHistories { get; set; } = new List<RankHistory>();
    }
}