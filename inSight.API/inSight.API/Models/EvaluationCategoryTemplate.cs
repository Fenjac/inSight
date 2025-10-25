using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    /// <summary>
    /// Master template for evaluation categories - reusable across evaluations
    /// </summary>
    public class EvaluationCategoryTemplate
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public QuestionnaireType QuestionnaireType { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        public bool IsManagementOnly { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<EvaluationQuestionTemplate> Questions { get; set; } = new List<EvaluationQuestionTemplate>();
    }
}
