namespace inSight.API.DTOs
{
    public class RankProgressionDto
    {
        public List<QuarterProgressionPoint> UserProgression { get; set; } = new();
        public List<QuarterProgressionPoint> TeamAverage { get; set; } = new();
        public List<QuarterProgressionPoint> CompanyAverage { get; set; } = new();
    }

    public class QuarterProgressionPoint
    {
        public string QuarterLabel { get; set; } = string.Empty; // "Q2 2024"
        public int QuarterId { get; set; }
        public string RankCode { get; set; } = string.Empty; // "P1", "P2", etc.
        public int RankValue { get; set; } // Numerical value 0-23 for chart Y-axis
        public int TotalScore { get; set; } // Cumulative score up to this quarter
        public int QuarterScore { get; set; } // Score earned in this quarter
    }
}
