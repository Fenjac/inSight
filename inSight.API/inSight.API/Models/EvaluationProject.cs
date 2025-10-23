using System;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class EvaluationProject
    {
        [Key]
        public Guid Id { get; set; }

        // Evaluacija
        public Guid EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; } = null!;

        // Projekat na kojem su sarađivali
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}