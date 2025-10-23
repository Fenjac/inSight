using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inSight.API.Models
{
    public class Evaluation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid QuarterId { get; set; }
        public Quarter Quarter { get; set; } = null!;

        [Required]
        public Guid EvaluatedUserId { get; set; }
        [ForeignKey("EvaluatedUserId")]
        public User EvaluatedUser { get; set; } = null!;

        [Required]
        public Guid EvaluatorUserId { get; set; }
        [ForeignKey("EvaluatorUserId")]
        public User EvaluatorUser { get; set; } = null!;

        [Required]
        public EvaluationType EvaluationType { get; set; }

        [Required]
        public QuestionnaireType QuestionnaireType { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Column(TypeName = "decimal(3,2)")]
        public decimal? OverallScore { get; set; }

        public string? GeneralComment { get; set; }

        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<EvaluationAnswer> Answers { get; set; } = new List<EvaluationAnswer>();
        public ICollection<ManagementReview> ManagementReviews { get; set; } = new List<ManagementReview>();

        // Projects za koje se odnosi ova evaluacija
        public ICollection<EvaluationProject> EvaluationProjects { get; set; } = new List<EvaluationProject>();
    }

    public enum EvaluationType
    {
        EmployeeToLead = 1,
        LeadToEmployee = 2
    }

    public enum QuestionnaireType
    {
        DEV = 1,
        QA = 2,
        APP = 3,
        DevOps = 4,
        TeamLead = 5
    }
}