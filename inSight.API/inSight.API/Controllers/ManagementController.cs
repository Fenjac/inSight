using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;
using inSight.API.Models;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/management/quarter/{quarterId}/overview
        // Vraća listu svih zaposlenih i njihov status evaluacija za dati kvartal
        [HttpGet("quarter/{quarterId}/overview")]
        public async Task<ActionResult> GetQuarterOverview(Guid quarterId)
        {
            var quarter = await _context.Quarters.FindAsync(quarterId);
            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            // Dohvati sve zaposlene (aktivne)
            var employees = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                .Where(u => u.IsActive)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    Role = u.Role != null ? u.Role.Name : null,
                    CurrentRank = u.CurrentRank != null ? u.CurrentRank.Code : null,
                    TeamId = u.TeamId,
                    TeamName = u.Team != null ? u.Team.Name : null
                })
                .ToListAsync();

            // Dohvati sve evaluacije za ovaj kvartal
            var evaluations = await _context.Evaluations
                .Where(e => e.QuarterId == quarterId)
                .Include(e => e.EvaluatorUser)
                .ToListAsync();

            // Grupišemo evaluacije po evaluiranom korisniku
            var evaluationsByUser = evaluations
                .GroupBy(e => e.EvaluatedUserId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Kreiraj pregled za svaki zaposleni
            var overview = employees.Select(emp => new
            {
                EmployeeId = emp.Id,
                FullName = $"{emp.FirstName} {emp.LastName}",
                emp.Email,
                emp.Role,
                Position = emp.Role, // Position je sada isto kao Role
                emp.CurrentRank,
                emp.TeamId,
                emp.TeamName,

                // Statistika evaluacija
                TotalEvaluations = evaluationsByUser.ContainsKey(emp.Id)
                    ? evaluationsByUser[emp.Id].Count
                    : 0,
                CompletedEvaluations = evaluationsByUser.ContainsKey(emp.Id)
                    ? evaluationsByUser[emp.Id].Count(e => e.IsCompleted)
                    : 0,
                PendingEvaluations = evaluationsByUser.ContainsKey(emp.Id)
                    ? evaluationsByUser[emp.Id].Count(e => !e.IsCompleted)
                    : 0,

                // Lista evaluatora
                Evaluators = evaluationsByUser.ContainsKey(emp.Id)
                    ? evaluationsByUser[emp.Id].Select(e => new
                    {
                        EvaluationId = e.Id,
                        EvaluatorName = $"{e.EvaluatorUser.FirstName} {e.EvaluatorUser.LastName}",
                        EvaluatorId = e.EvaluatorUserId,
                        EvaluationType = e.EvaluationType.ToString(),
                        IsCompleted = e.IsCompleted,
                        OverallScore = e.OverallScore,
                        CompletedAt = e.CompletedAt
                    }).Cast<object>().ToList()
                    : new List<object>()
            })
            .OrderBy(e => e.FullName)
            .ToList();

            return Ok(new
            {
                Quarter = new
                {
                    quarter.Id,
                    quarter.Year,
                    quarter.QuarterNumber,
                    QuarterName = $"Q{quarter.QuarterNumber} {quarter.Year}",
                    quarter.StartDate,
                    quarter.EndDate,
                    quarter.IsActive,
                    quarter.IsLocked
                },
                TotalEmployees = employees.Count,
                TotalEvaluations = evaluations.Count,
                CompletedEvaluations = evaluations.Count(e => e.IsCompleted),
                PendingEvaluations = evaluations.Count(e => !e.IsCompleted),
                Employees = overview
            });
        }

        // GET: api/management/employee/{employeeId}/quarter/{quarterId}
        // Vraća sve evaluacije za zaposlenog u datom kvartalu
        [HttpGet("employee/{employeeId}/quarter/{quarterId}")]
        public async Task<ActionResult> GetEmployeeEvaluations(Guid employeeId, Guid quarterId)
        {
            var employee = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.SystemRole)
                .FirstOrDefaultAsync(u => u.Id == employeeId);

            if (employee == null)
                return NotFound(new { message = "Zaposleni nije pronađen." });

            var quarter = await _context.Quarters.FindAsync(quarterId);
            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            // Dohvati sve evaluacije za ovog zaposlenog u ovom kvartalu
            var evaluations = await _context.Evaluations
                .Where(e => e.EvaluatedUserId == employeeId && e.QuarterId == quarterId)
                .Include(e => e.EvaluatorUser)
                    .ThenInclude(u => u.SystemRole)
                .Include(e => e.Quarter)
                .OrderByDescending(e => e.CompletedAt)
                .Select(e => new
                {
                    EvaluationId = e.Id,
                    EvaluatorId = e.EvaluatorUserId,
                    EvaluatorName = $"{e.EvaluatorUser.FirstName} {e.EvaluatorUser.LastName}",
                    EvaluatorRole = e.EvaluatorUser.SystemRole != null ? e.EvaluatorUser.SystemRole.Name : "N/A",
                    EvaluationType = e.EvaluationType.ToString(),
                    QuestionnaireType = e.QuestionnaireType.ToString(),
                    IsCompleted = e.IsCompleted,
                    OverallScore = e.OverallScore,
                    GeneralComment = e.GeneralComment,
                    CompletedAt = e.CompletedAt,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return Ok(new
            {
                Employee = new
                {
                    employee.Id,
                    FullName = $"{employee.FirstName} {employee.LastName}",
                    employee.Email,
                    Role = employee.SystemRole != null ? employee.SystemRole.Name : "N/A",
                    Position = employee.Role != null ? employee.Role.Name : "N/A",
                    CurrentRank = employee.CurrentRank != null ? employee.CurrentRank.Code : null
                },
                Quarter = new
                {
                    quarter.Id,
                    QuarterName = $"Q{quarter.QuarterNumber} {quarter.Year}",
                    quarter.Year,
                    quarter.QuarterNumber
                },
                Evaluations = evaluations
            });
        }

        // GET: api/management/evaluation/{evaluationId}/details
        // Vraća kompletne detalje evaluacije sa svim pitanjima, ocenama i komentarima
        [HttpGet("evaluation/{evaluationId}/details")]
        public async Task<ActionResult> GetEvaluationDetails(Guid evaluationId)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Quarter)
                .Include(e => e.EvaluatedUser)
                    .ThenInclude(u => u.Role)
                .Include(e => e.EvaluatedUser)
                    .ThenInclude(u => u.SystemRole)
                .Include(e => e.EvaluatorUser)
                    .ThenInclude(u => u.SystemRole)
                .Include(e => e.Answers)
                    .ThenInclude(a => a.Question)
                        .ThenInclude(q => q.Category)
                .FirstOrDefaultAsync(e => e.Id == evaluationId);

            if (evaluation == null)
                return NotFound(new { message = "Evaluacija nije pronađena." });

            // Grupiši odgovore po kategorijama
            var categories = evaluation.Answers
                .GroupBy(a => a.Question.Category)
                .Select(g => new
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    OrderIndex = g.Key.OrderIndex,
                    IsManagementOnly = g.Key.IsManagementOnly,
                    Questions = g.Select(a => new
                    {
                        QuestionId = a.Question.Id,
                        QuestionText = a.Question.QuestionText,
                        OrderIndex = a.Question.OrderIndex,
                        Score = a.Score,
                        Comment = a.Comment
                    })
                    .OrderBy(q => q.OrderIndex)
                    .ToList()
                })
                .OrderBy(c => c.OrderIndex)
                .ToList();

            return Ok(new
            {
                EvaluationId = evaluation.Id,
                Quarter = new
                {
                    evaluation.Quarter.Id,
                    QuarterName = $"Q{evaluation.Quarter.QuarterNumber} {evaluation.Quarter.Year}",
                    evaluation.Quarter.Year,
                    evaluation.Quarter.QuarterNumber
                },
                EvaluatedUser = new
                {
                    evaluation.EvaluatedUser.Id,
                    FullName = $"{evaluation.EvaluatedUser.FirstName} {evaluation.EvaluatedUser.LastName}",
                    evaluation.EvaluatedUser.Email,
                    Role = evaluation.EvaluatedUser.SystemRole != null ? evaluation.EvaluatedUser.SystemRole.Name : "N/A",
                    Position = evaluation.EvaluatedUser.Role != null ? evaluation.EvaluatedUser.Role.Name : "N/A"
                },
                EvaluatorUser = new
                {
                    evaluation.EvaluatorUser.Id,
                    FullName = $"{evaluation.EvaluatorUser.FirstName} {evaluation.EvaluatorUser.LastName}",
                    evaluation.EvaluatorUser.Email,
                    Role = evaluation.EvaluatorUser.SystemRole != null ? evaluation.EvaluatorUser.SystemRole.Name : "N/A"
                },
                EvaluationType = evaluation.EvaluationType.ToString(),
                QuestionnaireType = evaluation.QuestionnaireType.ToString(),
                IsCompleted = evaluation.IsCompleted,
                OverallScore = evaluation.OverallScore,
                GeneralComment = evaluation.GeneralComment,
                CompletedAt = evaluation.CompletedAt,
                Categories = categories
            });
        }

        // GET: api/management/quarters
        // Vraća listu svih kvartala sortiranih od najnovijeg
        [HttpGet("quarters")]
        public async Task<ActionResult> GetQuarters()
        {
            var quarters = await _context.Quarters
                .OrderByDescending(q => q.Year)
                .ThenByDescending(q => q.QuarterNumber)
                .Select(q => new
                {
                    q.Id,
                    q.Year,
                    q.QuarterNumber,
                    QuarterName = $"Q{q.QuarterNumber} {q.Year}",
                    q.StartDate,
                    q.EndDate,
                    q.IsActive,
                    q.IsLocked,
                    // Broj evaluacija za ovaj kvartal
                    TotalEvaluations = _context.Evaluations.Count(e => e.QuarterId == q.Id),
                    CompletedEvaluations = _context.Evaluations.Count(e => e.QuarterId == q.Id && e.IsCompleted)
                })
                .ToListAsync();

            return Ok(quarters);
        }
    }
}