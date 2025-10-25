using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inSight.API.Models
{
    public class EvaluationAnswer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; } = null!;

        [Required]
        public Guid QuestionInstanceId { get; set; }
        public EvaluationQuestionInstance QuestionInstance { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Score { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}