namespace inSight.API.DTOs
{
    public class QuarterDto
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int QuarterNumber { get; set; }
        public string QuarterName => $"Q{QuarterNumber} {Year}";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
    }

    public class CreateQuarterDto
    {
        public int Year { get; set; }
        public int QuarterNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}