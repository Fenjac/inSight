using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;
using inSight.API.DTOs;
using inSight.API.Models;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EvaluationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/evaluations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetEvaluations([FromQuery] Guid? quarterId = null)
        {
            var query = _context.Evaluations
                .Include(e => e.Quarter)
                .Include(e => e.EvaluatedUser)
                .Include(e => e.EvaluatorUser)
                .AsQueryable();

            if (quarterId.HasValue)
            {
                query = query.Where(e => e.QuarterId == quarterId.Value);
            }

            var evaluations = await query
                .Select(e => new EvaluationDto
                {
                    Id = e.Id,
                    QuarterId = e.QuarterId,
                    QuarterName = $"Q{e.Quarter.QuarterNumber} {e.Quarter.Year}",
                    EvaluatedUserId = e.EvaluatedUserId,
                    EvaluatedUserName = $"{e.EvaluatedUser.FirstName} {e.EvaluatedUser.LastName}",
                    EvaluatorUserId = e.EvaluatorUserId,
                    EvaluatorUserName = $"{e.EvaluatorUser.FirstName} {e.EvaluatorUser.LastName}",
                    EvaluationType = e.EvaluationType.ToString(),
                    QuestionnaireType = e.QuestionnaireType.ToString(),
                    IsCompleted = e.IsCompleted,
                    OverallScore = e.OverallScore,
                    GeneralComment = e.GeneralComment,
                    CompletedAt = e.CompletedAt
                })
                .ToListAsync();

            return Ok(evaluations);
        }

        // GET: api/evaluations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationDto>> GetEvaluation(Guid id)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Quarter)
                .Include(e => e.EvaluatedUser)
                .Include(e => e.EvaluatorUser)
                .Include(e => e.Answers)
                    .ThenInclude(a => a.QuestionInstance)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evaluation == null)
                return NotFound(new { message = "Ocenjivanje nije pronađeno." });

            // Grupiši pitanja po kategorijama
            var categories = evaluation.Answers
                .GroupBy(a => new {
                    CategoryName = a.QuestionInstance.CategoryName,
                    CategoryOrderIndex = a.QuestionInstance.CategoryOrderIndex
                })
                .Select(g => new EvaluationCategoryDto
                {
                    CategoryId = Guid.Empty, // Instance doesn't have category ID
                    CategoryName = g.Key.CategoryName,
                    OrderIndex = g.Key.CategoryOrderIndex,
                    IsManagementOnly = false, // TODO: Store this in instance if needed
                    Questions = g.Select(a => new EvaluationQuestionDto
                    {
                        QuestionId = a.QuestionInstance.Id,
                        QuestionText = a.QuestionInstance.QuestionText,
                        OrderIndex = a.QuestionInstance.QuestionOrderIndex,
                        Score = a.Score,
                        Comment = a.Comment
                    }).OrderBy(q => q.OrderIndex).ToList()
                })
                .OrderBy(c => c.OrderIndex)
                .ToList();

            var evaluationDto = new EvaluationDto
            {
                Id = evaluation.Id,
                QuarterId = evaluation.QuarterId,
                QuarterName = $"Q{evaluation.Quarter.QuarterNumber} {evaluation.Quarter.Year}",
                EvaluatedUserId = evaluation.EvaluatedUserId,
                EvaluatedUserName = $"{evaluation.EvaluatedUser.FirstName} {evaluation.EvaluatedUser.LastName}",
                EvaluatorUserId = evaluation.EvaluatorUserId,
                EvaluatorUserName = $"{evaluation.EvaluatorUser.FirstName} {evaluation.EvaluatorUser.LastName}",
                EvaluationType = evaluation.EvaluationType.ToString(),
                QuestionnaireType = evaluation.QuestionnaireType.ToString(),
                IsCompleted = evaluation.IsCompleted,
                OverallScore = evaluation.OverallScore,
                GeneralComment = evaluation.GeneralComment,
                CompletedAt = evaluation.CompletedAt,
                Categories = categories
            };

            return Ok(evaluationDto);
        }

        // POST: api/evaluations
        [HttpPost]
        public async Task<ActionResult<EvaluationDto>> CreateEvaluation(CreateEvaluationDto dto)
        {
            // Proveri da li postoji kvartal
            var quarter = await _context.Quarters.FindAsync(dto.QuarterId);
            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            if (quarter.IsLocked)
                return BadRequest(new { message = "Kvartal je zaključan i ne mogu se kreirati nova ocenjivanja." });

            // Proveri da li postoje korisnici
            var evaluatedUser = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == dto.EvaluatedUserId);

            var evaluatorUser = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == dto.EvaluatorUserId);

            if (evaluatedUser == null || evaluatorUser == null)
                return NotFound(new { message = "Korisnik nije pronađen." });

            // Odredi tip ocenjivanja i upitnika
            EvaluationType evaluationType;
            QuestionnaireType questionnaireType;

            var evaluatorSystemRole = evaluatorUser.SystemRole?.Name;
            var evaluatedSystemRole = evaluatedUser.SystemRole?.Name;
            var evaluatedJobRole = evaluatedUser.Role?.Name;

            if (evaluatorSystemRole == "TeamLead" && evaluatedSystemRole == "Employee")
            {
                evaluationType = EvaluationType.LeadToEmployee;
                questionnaireType = evaluatedJobRole switch
                {
                    "Developer" => QuestionnaireType.DEV,
                    "QA" => QuestionnaireType.QA,
                    "Application Engineer" => QuestionnaireType.APP,
                    _ => throw new Exception("Nevažeća pozicija zaposlenog")
                };
            }
            else if (evaluatedSystemRole == "TeamLead" && evaluatorSystemRole == "Employee")
            {
                evaluationType = EvaluationType.EmployeeToLead;
                questionnaireType = QuestionnaireType.TeamLead;
            }
            else
            {
                return BadRequest(new { message = "Nevalidna kombinacija evaluatora i ocenjenog." });
            }

            // Proveri da li već postoji evaluation
            var exists = await _context.Evaluations.AnyAsync(e =>
                e.QuarterId == dto.QuarterId &&
                e.EvaluatedUserId == dto.EvaluatedUserId &&
                e.EvaluatorUserId == dto.EvaluatorUserId);

            if (exists)
                return BadRequest(new { message = "Ocenjivanje već postoji za ovaj kvartal." });

            var evaluation = new Evaluation
            {
                QuarterId = dto.QuarterId,
                EvaluatedUserId = dto.EvaluatedUserId,
                EvaluatorUserId = dto.EvaluatorUserId,
                EvaluationType = evaluationType,
                QuestionnaireType = questionnaireType,
                IsCompleted = false
            };

            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvaluation), new { id = evaluation.Id }, new { id = evaluation.Id });
        }

        // GET: api/evaluations/{id}/form
        [HttpGet("{id}/form")]
        public async Task<ActionResult<EvaluationDto>> GetEvaluationForm(Guid id)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Quarter)
                .Include(e => e.EvaluatedUser)
                .Include(e => e.EvaluatorUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evaluation == null)
                return NotFound(new { message = "Ocenjivanje nije pronađeno." });

            // Dohvati sve kategorije i pitanja za ovaj tip upitnika
            var categories = await _context.EvaluationCategories
                .Where(c => c.QuestionnaireType == evaluation.QuestionnaireType)
                .Include(c => c.Questions)
                .OrderBy(c => c.OrderIndex)
                .ToListAsync();

            // Dohvati već date odgovore ako postoje
            var existingAnswers = await _context.EvaluationAnswers
                .Where(a => a.EvaluationId == id)
                .ToDictionaryAsync(a => a.QuestionId, a => a);

            var categoriesDto = categories.Select(c => new EvaluationCategoryDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                OrderIndex = c.OrderIndex,
                IsManagementOnly = c.IsManagementOnly,
                Questions = c.Questions.OrderBy(q => q.OrderIndex).Select(q => new EvaluationQuestionDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    OrderIndex = q.OrderIndex,
                    Score = existingAnswers.ContainsKey(q.Id) ? existingAnswers[q.Id].Score : null,
                    Comment = existingAnswers.ContainsKey(q.Id) ? existingAnswers[q.Id].Comment : null
                }).ToList()
            }).ToList();

            var evaluationDto = new EvaluationDto
            {
                Id = evaluation.Id,
                QuarterId = evaluation.QuarterId,
                QuarterName = $"Q{evaluation.Quarter.QuarterNumber} {evaluation.Quarter.Year}",
                EvaluatedUserId = evaluation.EvaluatedUserId,
                EvaluatedUserName = $"{evaluation.EvaluatedUser.FirstName} {evaluation.EvaluatedUser.LastName}",
                EvaluatorUserId = evaluation.EvaluatorUserId,
                EvaluatorUserName = $"{evaluation.EvaluatorUser.FirstName} {evaluation.EvaluatorUser.LastName}",
                EvaluationType = evaluation.EvaluationType.ToString(),
                QuestionnaireType = evaluation.QuestionnaireType.ToString(),
                IsCompleted = evaluation.IsCompleted,
                OverallScore = evaluation.OverallScore,
                GeneralComment = evaluation.GeneralComment,
                Categories = categoriesDto
            };

            return Ok(evaluationDto);
        }

        // POST: api/evaluations/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitEvaluation(SubmitEvaluationDto dto)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Quarter)
                .FirstOrDefaultAsync(e => e.Id == dto.EvaluationId);

            if (evaluation == null)
                return NotFound(new { message = "Ocenjivanje nije pronađeno." });

            if (evaluation.Quarter.IsLocked)
                return BadRequest(new { message = "Kvartal je zaključan." });

            // Obriši stare odgovore ako postoje
            var oldAnswers = await _context.EvaluationAnswers
                .Where(a => a.EvaluationId == dto.EvaluationId)
                .ToListAsync();
            _context.EvaluationAnswers.RemoveRange(oldAnswers);

            // Dodaj nove odgovore
            foreach (var answerDto in dto.Answers)
            {
                var answer = new EvaluationAnswer
                {
                    EvaluationId = dto.EvaluationId,
                    QuestionId = answerDto.QuestionId,
                    Score = answerDto.Score,
                    Comment = answerDto.Comment
                };
                _context.EvaluationAnswers.Add(answer);
            }

            // Izračunaj prosečnu ocenu
            var averageScore = dto.Answers.Average(a => a.Score);
            evaluation.OverallScore = (decimal)averageScore;
            evaluation.GeneralComment = dto.GeneralComment;
            evaluation.IsCompleted = true;
            evaluation.CompletedAt = DateTime.UtcNow;
            evaluation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Ocenjivanje je uspešno sačuvano.", overallScore = evaluation.OverallScore });
        }
    }
}