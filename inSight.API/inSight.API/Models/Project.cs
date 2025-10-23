using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Project assignments (ko radi na projektu)
        public ICollection<ProjectAssignment> Assignments { get; set; } = new List<ProjectAssignment>();

        // Evaluations vezane za ovaj projekat
        public ICollection<EvaluationProject> EvaluationProjects { get; set; } = new List<EvaluationProject>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}