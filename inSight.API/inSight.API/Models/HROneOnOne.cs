using System.ComponentModel.DataAnnotations;

namespace inSight.API.Models
{
    public class HROneOnOne
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EmployeeId { get; set; }
        public User Employee { get; set; } = null!;

        [Required]
        public Guid HRRepresentativeId { get; set; }
        public User HRRepresentative { get; set; } = null!;

        [Required]
        public DateTime MeetingDate { get; set; }

        public string? Topics { get; set; }

        public string? Notes { get; set; }

        public string? ActionItems { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}