using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class EmailNotification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public EmailType EmailType { get; set; }

        [Required]
        [MaxLength(500)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required]
        public EmailStatus Status { get; set; } = EmailStatus.Pending;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public string? ErrorMessage { get; set; }

        public int RetryCount { get; set; } = 0;
    }

    public enum EmailType
    {
        QuarterReminder = 1,
        QuarterStart = 2,
        EvaluationReady = 3,
        FeedbackReady = 4,
        RankChanged = 5
    }

    public enum EmailStatus
    {
        Pending = 1,
        Sent = 2,
        Failed = 3
    }
}