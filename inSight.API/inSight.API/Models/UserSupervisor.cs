using System;
using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class UserSupervisor
    {
        [Key]
        public Guid Id { get; set; }

        // Zaposleni
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Nadređeni
        public Guid SupervisorId { get; set; }
        public User Supervisor { get; set; } = null!;

        // Tip nadređenog
        public SupervisorType SupervisorType { get; set; }

        // Period važenja
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}