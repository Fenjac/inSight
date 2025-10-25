using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    /// <summary>
    /// Master template for evaluation questions - reusable across evaluations
    /// </summary>
    public class EvaluationQuestionTemplate
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CategoryTemplateId { get; set; }
        public EvaluationCategoryTemplate CategoryTemplate { get; set; } = null!;

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public int OrderIndex { get; set; }

        public bool IsManagementOnly { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<EvaluationQuestionInstance> QuestionInstances { get; set; } = new List<EvaluationQuestionInstance>();
    }
}
