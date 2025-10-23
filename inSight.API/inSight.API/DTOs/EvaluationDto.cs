namespace inSight.API.DTOs
{
    public class EvaluationDto
    {
        public Guid Id { get; set; }
        public Guid QuarterId { get; set; }
        public string QuarterName { get; set; } = string.Empty;
        public Guid EvaluatedUserId { get; set; }
        public string EvaluatedUserName { get; set; } = string.Empty;
        public Guid EvaluatorUserId { get; set; }
        public string EvaluatorUserName { get; set; } = string.Empty;
        public string EvaluationType { get; set; } = string.Empty;
        public string QuestionnaireType { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public decimal? OverallScore { get; set; }
        public string? GeneralComment { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<EvaluationCategoryDto> Categories { get; set; } = new();
    }

    public class EvaluationCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsManagementOnly { get; set; }
        public List<EvaluationQuestionDto> Questions { get; set; } = new();
    }

    public class EvaluationQuestionDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public int? Score { get; set; }
        public string? Comment { get; set; }
    }

    public class SubmitEvaluationDto
    {
        public Guid EvaluationId { get; set; }
        public string? GeneralComment { get; set; }
        public List<EvaluationAnswerDto> Answers { get; set; } = new();
    }

    public class EvaluationAnswerDto
    {
        public Guid QuestionId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
    }

    public class CreateEvaluationDto
    {
        public Guid QuarterId { get; set; }
        public Guid EvaluatedUserId { get; set; }
        public Guid EvaluatorUserId { get; set; }
    }
}