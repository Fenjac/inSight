using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    /// <summary>
    /// Snapshot copy of a question for a specific evaluation
    /// This allows questions to change over time without affecting historical evaluations
    /// </summary>
    public class EvaluationQuestionInstance
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; } = null!;

        // Reference to original template (for tracking purposes, can be null if template deleted)
        public Guid? TemplateQuestionId { get; set; }
        public EvaluationQuestionTemplate? TemplateQuestion { get; set; }

        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public int CategoryOrderIndex { get; set; }

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public int QuestionOrderIndex { get; set; }

        public bool IsManagementOnly { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public EvaluationAnswer? Answer { get; set; }
    }
}
