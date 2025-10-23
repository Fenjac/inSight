using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // System Role (Employee, TeamLead, HR, CTO, CEO) - za permisije
        public Guid? SystemRoleId { get; set; }
        public SystemRole? SystemRole { get; set; }

        // Job Role (Developer, QA, Application Engineer) - za poziciju
        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }

        // Current Rank (P1, J1, M1, S1, E1, etc.)
        public Guid? CurrentRankId { get; set; }
        public Rank? CurrentRank { get; set; }

        // Team membership
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties

        // Evaluations
        public ICollection<Evaluation> EvaluationsAsEvaluated { get; set; } = new List<Evaluation>();
        public ICollection<Evaluation> EvaluationsAsEvaluator { get; set; } = new List<Evaluation>();

        // Supervisors (ko je meni nadređeni)
        public ICollection<UserSupervisor> Supervisors { get; set; } = new List<UserSupervisor>();

        // Subordinates (ko je meni podređen)
        public ICollection<UserSupervisor> Subordinates { get; set; } = new List<UserSupervisor>();

        // Project assignments
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();

        // Teams I lead (ako sam Team Lead)
        public ICollection<Team> TeamsLed { get; set; } = new List<Team>();
    }
}