using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class ManagementReview
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; } = null!;

        [Required]
        public Guid ReviewedById { get; set; }
        public User ReviewedBy { get; set; } = null!;

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}