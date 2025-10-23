using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard/stats
        [HttpGet("stats")]
        public async Task<ActionResult> GetDashboardStats()
        {
            // Total users
            var totalUsers = await _context.Users.CountAsync(u => u.IsActive);

            // Active quarter
            var activeQuarter = await _context.Quarters
                .Where(q => q.IsActive)
                .Select(q => new
                {
                    q.Id,
                    q.Year,
                    q.QuarterNumber,
                    QuarterName = $"Q{q.QuarterNumber} {q.Year}"
                })
                .FirstOrDefaultAsync();

            // Evaluations (for active quarter if exists)
            var totalEvaluations = 0;
            var completedEvaluations = 0;
            if (activeQuarter != null)
            {
                totalEvaluations = await _context.Evaluations
                    .CountAsync(e => e.QuarterId == activeQuarter.Id);
                completedEvaluations = await _context.Evaluations
                    .CountAsync(e => e.QuarterId == activeQuarter.Id && e.IsCompleted);
            }

            // Teams
            var totalTeams = await _context.Teams.CountAsync(t => t.IsActive);

            // Active projects
            var activeProjects = await _context.Projects.CountAsync(p => p.IsActive);

            // Team leads (users with SystemRole = "TeamLead")
            var teamLeads = await _context.Users
                .Include(u => u.SystemRole)
                .CountAsync(u => u.IsActive && u.SystemRole != null && u.SystemRole.Name == "TeamLead");

            // Users by team
            var usersByTeam = await _context.Teams
                .Where(t => t.IsActive)
                .Select(t => new
                {
                    TeamName = t.Name,
                    UserCount = t.Members.Count(m => m.IsActive)
                })
                .OrderByDescending(t => t.UserCount)
                .ToListAsync();

            // Users by role (job role)
            var usersByRole = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.IsActive && u.Role != null)
                .GroupBy(u => u.Role!.Name)
                .Select(g => new
                {
                    RoleName = g.Key,
                    UserCount = g.Count()
                })
                .OrderByDescending(r => r.UserCount)
                .ToListAsync();

            // Users by rank level (P, J, M, S, E)
            var usersByRankLevel = await _context.Users
                .Include(u => u.CurrentRank)
                .Where(u => u.IsActive && u.CurrentRank != null)
                .ToListAsync();

            var rankLevelStats = usersByRankLevel
                .GroupBy(u => u.CurrentRank!.Code.Substring(0, 1)) // First letter (P, J, M, S, E)
                .Select(g => new
                {
                    RankLevel = g.Key switch
                    {
                        "P" => "Praktikant",
                        "J" => "Junior",
                        "M" => "Medior",
                        "S" => "Senior",
                        "E" => "Expert",
                        _ => "Unknown"
                    },
                    UserCount = g.Count()
                })
                .OrderBy(r => r.RankLevel)
                .ToList();

            return Ok(new
            {
                TotalUsers = totalUsers,
                ActiveQuarter = activeQuarter,
                TotalEvaluations = totalEvaluations,
                CompletedEvaluations = completedEvaluations,
                PendingEvaluations = totalEvaluations - completedEvaluations,
                TotalTeams = totalTeams,
                ActiveProjects = activeProjects,
                TeamLeads = teamLeads,
                UsersByTeam = usersByTeam,
                UsersByRole = usersByRole,
                UsersByRankLevel = rankLevelStats
            });
        }
    }
}