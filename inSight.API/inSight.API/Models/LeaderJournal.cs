using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class LeaderJournal
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid LeaderId { get; set; }
        public User Leader { get; set; } = null!;

        [Required]
        public Guid EmployeeId { get; set; }
        public User Employee { get; set; } = null!;

        [Required]
        public Guid QuarterId { get; set; }
        public Quarter Quarter { get; set; } = null!;

        [Required]
        public DateTime EntryDate { get; set; }

        [Required]
        public JournalEntryType EntryType { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum JournalEntryType
    {
        Positive = 1,
        Negative = 2,
        Neutral = 3
    }
}