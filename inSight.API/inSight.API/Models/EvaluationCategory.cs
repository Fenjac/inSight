using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class EvaluationCategory
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

        // Navigation properties
        public ICollection<EvaluationQuestion> Questions { get; set; } = new List<EvaluationQuestion>();
    }
}