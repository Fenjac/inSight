using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class EvaluationQuestion
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CategoryId { get; set; }
        public EvaluationCategory Category { get; set; } = null!;

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public int OrderIndex { get; set; }

        public bool IsManagementOnly { get; set; } = false;

        // Navigation properties
        public ICollection<EvaluationAnswer> Answers { get; set; } = new List<EvaluationAnswer>();
    }
}