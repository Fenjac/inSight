using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;
using inSight.API.DTOs;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.SystemRole != null ? u.SystemRole.Name : "N/A",
                    Position = u.Role != null ? u.Role.Name : "N/A",
                    CurrentRank = u.CurrentRank != null ? u.CurrentRank.Code : null,
                    StartingScore = u.StartingScore,
                    CurrentTotalScore = u.CurrentTotalScore,
                    IsActive = u.IsActive,
                    TeamLeadId = u.TeamId,
                    TeamLeadName = u.Team != null ? u.Team.Name : null,
                    SystemRoleId = u.SystemRoleId,
                    RoleId = u.RoleId,
                    CurrentRankId = u.CurrentRankId,
                    TeamId = u.TeamId,
                    // Nested objects for display
                    SystemRoleDetails = u.SystemRole != null ? new SystemRoleDto
                    {
                        Id = u.SystemRole.Id,
                        Name = u.SystemRole.Name
                    } : null,
                    RoleDetails = u.Role != null ? new RoleDto
                    {
                        Id = u.Role.Id,
                        Name = u.Role.Name
                    } : null,
                    CurrentRankDetails = u.CurrentRank != null ? new RankDto
                    {
                        Id = u.CurrentRank.Id,
                        Code = u.CurrentRank.Code,
                        Name = u.CurrentRank.Name
                    } : null,
                    TeamName = u.Team != null ? u.Team.Name : null
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithTeamDto>> GetUser(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                    .ThenInclude(t => t.Members)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Korisnik nije pronađen." });

            var userDto = new UserWithTeamDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.SystemRole != null ? user.SystemRole.Name : "N/A",
                Position = user.Role != null ? user.Role.Name : "N/A",
                CurrentRank = user.CurrentRank != null ? user.CurrentRank.Code : null,
                StartingScore = user.StartingScore,
                CurrentTotalScore = user.CurrentTotalScore,
                IsActive = user.IsActive,
                TeamLeadId = user.TeamId,
                TeamLeadName = user.Team != null ? user.Team.Name : null,
                SystemRoleId = user.SystemRoleId,
                RoleId = user.RoleId,
                CurrentRankId = user.CurrentRankId,
                TeamId = user.TeamId,
                CreatedAt = user.CreatedAt,
                // Nested objects for frontend
                SystemRoleDetails = user.SystemRole != null ? new SystemRoleDto
                {
                    Id = user.SystemRole.Id,
                    Name = user.SystemRole.Name
                } : null,
                RoleDetails = user.Role != null ? new RoleDto
                {
                    Id = user.Role.Id,
                    Name = user.Role.Name
                } : null,
                CurrentRankDetails = user.CurrentRank != null ? new RankDto
                {
                    Id = user.CurrentRank.Id,
                    Code = user.CurrentRank.Code,
                    Name = user.CurrentRank.Name
                } : null,
                TeamName = user.Team != null ? user.Team.Name : null,
                TeamMembers = user.Team != null ? user.Team.Members.Select(tm => new UserDto
                {
                    Id = tm.Id,
                    FirstName = tm.FirstName,
                    LastName = tm.LastName,
                    Email = tm.Email,
                    Role = tm.SystemRole != null ? tm.SystemRole.Name : "N/A",
                    Position = tm.Role != null ? tm.Role.Name : "N/A",
                    CurrentRank = tm.CurrentRank != null ? tm.CurrentRank.Code : null,
                    StartingScore = tm.StartingScore,
                    CurrentTotalScore = tm.CurrentTotalScore,
                    IsActive = tm.IsActive
                }).ToList() : new List<UserDto>()
            };

            return Ok(userDto);
        }

        // GET: api/users/team-leads
        [HttpGet("team-leads")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetTeamLeads()
        {
            var teamLeads = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Where(u => u.SystemRole != null && u.SystemRole.Name == "TeamLead" && u.IsActive)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.SystemRole.Name,
                    Position = u.Role != null ? u.Role.Name : "N/A",
                    CurrentRank = u.CurrentRank != null ? u.CurrentRank.Code : null,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            return Ok(teamLeads);
        }

        // GET: api/users/employees
        [HttpGet("employees")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetEmployees()
        {
            var employees = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                .Where(u => u.SystemRole != null && u.SystemRole.Name == "Employee" && u.IsActive)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.SystemRole.Name,
                    Position = u.Role != null ? u.Role.Name : "N/A",
                    CurrentRank = u.CurrentRank != null ? u.CurrentRank.Code : null,
                    IsActive = u.IsActive,
                    TeamLeadId = u.TeamId,
                    TeamLeadName = u.Team != null ? u.Team.Name : null
                })
                .ToListAsync();

            return Ok(employees);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
        {
            // Validate email uniqueness
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Korisnik sa ovim email-om već postoji." });
            }

            // Find SystemRole by ID
            if (!Guid.TryParse(dto.SystemRoleId, out var systemRoleGuid))
            {
                return BadRequest(new { message = "Nevalidan ID sistemske role." });
            }
            var systemRole = await _context.SystemRoles.FindAsync(systemRoleGuid);
            if (systemRole == null)
            {
                return BadRequest(new { message = "Sistemska rola nije pronađena." });
            }

            // Find Role by ID
            if (!Guid.TryParse(dto.RoleId, out var roleGuid))
            {
                return BadRequest(new { message = "Nevalidan ID pozicije." });
            }
            var role = await _context.Roles.FindAsync(roleGuid);
            if (role == null)
            {
                return BadRequest(new { message = "Pozicija nije pronađena." });
            }

            // Create new user
            var user = new Models.User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                SystemRoleId = systemRole.Id,
                RoleId = role.Id,
                CurrentRankId = !string.IsNullOrEmpty(dto.CurrentRankId) && Guid.TryParse(dto.CurrentRankId, out var rankGuid) ? rankGuid : null,
                TeamId = !string.IsNullOrEmpty(dto.TeamId) && Guid.TryParse(dto.TeamId, out var teamGuid) ? teamGuid : null,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return created user
            var createdUser = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                .Where(u => u.Id == user.Id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.SystemRole != null ? u.SystemRole.Name : "N/A",
                    Position = u.Role != null ? u.Role.Name : "N/A",
                    CurrentRank = u.CurrentRank != null ? u.CurrentRank.Code : null,
                    StartingScore = u.StartingScore,
                    CurrentTotalScore = u.CurrentTotalScore,
                    IsActive = u.IsActive,
                    TeamLeadId = u.TeamId,
                    TeamLeadName = u.Team != null ? u.Team.Name : null,
                    SystemRoleId = u.SystemRoleId,
                    RoleId = u.RoleId,
                    CurrentRankId = u.CurrentRankId,
                    TeamId = u.TeamId
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUser);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Korisnik nije pronađen." });
            }

            // Check email uniqueness (excluding current user)
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
            {
                return BadRequest(new { message = "Korisnik sa ovim email-om već postoji." });
            }

            // Update fields
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            // Update password only if provided
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            // Update SystemRole
            if (!string.IsNullOrEmpty(dto.SystemRoleId) && Guid.TryParse(dto.SystemRoleId, out var systemRoleGuid))
            {
                var systemRole = await _context.SystemRoles.FindAsync(systemRoleGuid);
                if (systemRole != null)
                {
                    user.SystemRoleId = systemRole.Id;
                }
            }

            // Update Role
            if (!string.IsNullOrEmpty(dto.RoleId) && Guid.TryParse(dto.RoleId, out var roleGuid))
            {
                var role = await _context.Roles.FindAsync(roleGuid);
                if (role != null)
                {
                    user.RoleId = role.Id;
                }
            }

            // Update Rank
            if (!string.IsNullOrEmpty(dto.CurrentRankId) && Guid.TryParse(dto.CurrentRankId, out var rankGuid))
            {
                user.CurrentRankId = rankGuid;
            }
            else if (string.IsNullOrEmpty(dto.CurrentRankId))
            {
                user.CurrentRankId = null;
            }

            // Update Team
            if (!string.IsNullOrEmpty(dto.TeamId) && Guid.TryParse(dto.TeamId, out var teamGuid))
            {
                user.TeamId = teamGuid;
            }
            else if (string.IsNullOrEmpty(dto.TeamId))
            {
                user.TeamId = null;
            }

            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Return updated user
            var updatedUser = await _context.Users
                .Include(u => u.SystemRole)
                .Include(u => u.Role)
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.SystemRole != null ? u.SystemRole.Name : "N/A",
                    Position = u.Role != null ? u.Role.Name : "N/A",
                    CurrentRank = u.CurrentRank != null ? u.CurrentRank.Code : null,
                    StartingScore = u.StartingScore,
                    CurrentTotalScore = u.CurrentTotalScore,
                    IsActive = u.IsActive,
                    TeamLeadId = u.TeamId,
                    TeamLeadName = u.Team != null ? u.Team.Name : null,
                    SystemRoleId = u.SystemRoleId,
                    RoleId = u.RoleId,
                    CurrentRankId = u.CurrentRankId,
                    TeamId = u.TeamId
                })
                .FirstOrDefaultAsync();

            return Ok(updatedUser);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Korisnik nije pronađen." });
            }

            // Soft delete - just set IsActive to false
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Korisnik uspešno obrisan." });
        }

        // GET: api/users/{id}/rank-progression
        [HttpGet("{id}/rank-progression")]
        public async Task<ActionResult<RankProgressionDto>> GetRankProgression(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.CurrentRank)
                .Include(u => u.Team)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Korisnik nije pronađen." });

            // Get all quarters ordered by date
            var quarters = await _context.Quarters
                .OrderBy(q => q.Year)
                .ThenBy(q => q.QuarterNumber)
                .ToListAsync();

            // Get all ranks (universal for all roles), ordered by OrderIndex
            var ranks = await _context.Ranks
                .OrderBy(r => r.OrderIndex)
                .ToListAsync();

            // Create rank code to value mapping using OrderIndex
            var rankValueMap = ranks
                .ToDictionary(r => r.Code, r => r.OrderIndex);

            // Get user's quarterly scores
            var userQuarterlyScores = await _context.QuarterlyScores
                .Where(qs => qs.UserId == id)
                .Include(qs => qs.Quarter)
                .OrderBy(qs => qs.Quarter.Year)
                .ThenBy(qs => qs.Quarter.QuarterNumber)
                .ToListAsync();

            // Build user progression
            var userProgression = new List<QuarterProgressionPoint>();
            int cumulativeScore = user.StartingScore;

            // Determine initial rank based on starting score
            var initialRankThresholds = await _context.RankScoreThresholds
                .Where(rst => rst.RoleId == user.RoleId)
                .Include(rst => rst.Rank)
                .ToListAsync();

            var initialRankThreshold = initialRankThresholds
                .Where(rt => user.StartingScore >= rt.MinScore && user.StartingScore <= rt.MaxScore)
                .FirstOrDefault();

            string currentRankCode = initialRankThreshold?.Rank.Code ?? ranks.FirstOrDefault()?.Code ?? "P1";

            foreach (var quarter in quarters)
            {
                var quarterScore = userQuarterlyScores.FirstOrDefault(qs => qs.QuarterId == quarter.Id);

                if (quarterScore != null)
                {
                    cumulativeScore += quarterScore.TotalScore;
                }

                // Determine rank based on cumulative score
                var rankThresholds = await _context.RankScoreThresholds
                    .Where(rst => rst.RoleId == user.RoleId)
                    .Include(rst => rst.Rank)
                    .ToListAsync();

                var currentRankThreshold = rankThresholds
                    .Where(rt => cumulativeScore >= rt.MinScore && cumulativeScore <= rt.MaxScore)
                    .FirstOrDefault();

                if (currentRankThreshold != null)
                {
                    currentRankCode = currentRankThreshold.Rank.Code;
                }

                userProgression.Add(new QuarterProgressionPoint
                {
                    QuarterLabel = $"Q{quarter.QuarterNumber} {quarter.Year}",
                    QuarterId = (int)quarter.QuarterNumber,
                    RankCode = currentRankCode,
                    RankValue = rankValueMap.ContainsKey(currentRankCode) ? rankValueMap[currentRankCode] : 0,
                    TotalScore = cumulativeScore,
                    QuarterScore = quarterScore?.TotalScore ?? 0
                });
            }

            // Calculate team average (if user has a team)
            var teamAverage = new List<QuarterProgressionPoint>();
            if (user.TeamId.HasValue)
            {
                var teamMembers = await _context.Users
                    .Where(u => u.TeamId == user.TeamId && u.RoleId == user.RoleId)
                    .ToListAsync();

                foreach (var quarter in quarters)
                {
                    var teamScores = new List<int>();
                    foreach (var member in teamMembers)
                    {
                        var memberQuarterlyScores = await _context.QuarterlyScores
                            .Where(qs => qs.UserId == member.Id && qs.Quarter.Year <= quarter.Year &&
                                         (qs.Quarter.Year < quarter.Year || qs.Quarter.QuarterNumber <= quarter.QuarterNumber))
                            .ToListAsync();

                        int memberCumulativeScore = member.StartingScore + memberQuarterlyScores.Sum(qs => qs.TotalScore);
                        teamScores.Add(memberCumulativeScore);
                    }

                    if (teamScores.Any())
                    {
                        int avgScore = (int)teamScores.Average();
                        var rankThresholds = await _context.RankScoreThresholds
                            .Where(rst => rst.RoleId == user.RoleId)
                            .Include(rst => rst.Rank)
                            .ToListAsync();

                        var avgRankThreshold = rankThresholds
                            .Where(rt => avgScore >= rt.MinScore && avgScore <= rt.MaxScore)
                            .FirstOrDefault();

                        string avgRankCode = avgRankThreshold?.Rank.Code ?? "P1";

                        teamAverage.Add(new QuarterProgressionPoint
                        {
                            QuarterLabel = $"Q{quarter.QuarterNumber} {quarter.Year}",
                            QuarterId = (int)quarter.QuarterNumber,
                            RankCode = avgRankCode,
                            RankValue = rankValueMap.ContainsKey(avgRankCode) ? rankValueMap[avgRankCode] : 0,
                            TotalScore = avgScore,
                            QuarterScore = 0
                        });
                    }
                }
            }

            // Calculate company average
            var companyAverage = new List<QuarterProgressionPoint>();
            var allUsersWithSameRole = await _context.Users
                .Where(u => u.RoleId == user.RoleId)
                .ToListAsync();

            foreach (var quarter in quarters)
            {
                var companyScores = new List<int>();
                foreach (var member in allUsersWithSameRole)
                {
                    var memberQuarterlyScores = await _context.QuarterlyScores
                        .Where(qs => qs.UserId == member.Id && qs.Quarter.Year <= quarter.Year &&
                                     (qs.Quarter.Year < quarter.Year || qs.Quarter.QuarterNumber <= quarter.QuarterNumber))
                        .ToListAsync();

                    int memberCumulativeScore = member.StartingScore + memberQuarterlyScores.Sum(qs => qs.TotalScore);
                    companyScores.Add(memberCumulativeScore);
                }

                if (companyScores.Any())
                {
                    int avgScore = (int)companyScores.Average();
                    var rankThresholds = await _context.RankScoreThresholds
                        .Where(rst => rst.RoleId == user.RoleId)
                        .Include(rst => rst.Rank)
                        .ToListAsync();

                    var avgRankThreshold = rankThresholds
                        .Where(rt => avgScore >= rt.MinScore && avgScore <= rt.MaxScore)
                        .FirstOrDefault();

                    string avgRankCode = avgRankThreshold?.Rank.Code ?? "P1";

                    companyAverage.Add(new QuarterProgressionPoint
                    {
                        QuarterLabel = $"Q{quarter.QuarterNumber} {quarter.Year}",
                        QuarterId = (int)quarter.QuarterNumber,
                        RankCode = avgRankCode,
                        RankValue = rankValueMap.ContainsKey(avgRankCode) ? rankValueMap[avgRankCode] : 0,
                        TotalScore = avgScore,
                        QuarterScore = 0
                    });
                }
            }

            return Ok(new RankProgressionDto
            {
                UserProgression = userProgression,
                TeamAverage = teamAverage,
                CompanyAverage = companyAverage
            });
        }

        // GET: api/users/{id}/quarterly-scores
        [HttpGet("{id}/quarterly-scores")]
        public async Task<ActionResult> GetUserQuarterlyScores(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Korisnik nije pronađen." });

            var quarterlyScores = await _context.QuarterlyScores
                .Where(qs => qs.UserId == id)
                .Include(qs => qs.Quarter)
                .OrderByDescending(qs => qs.Quarter.Year)
                .ThenByDescending(qs => qs.Quarter.QuarterNumber)
                .Select(qs => new
                {
                    id = qs.Id,
                    userId = qs.UserId,
                    quarter = qs.Quarter.QuarterNumber,
                    year = qs.Quarter.Year,
                    quarterLabel = $"Q{qs.Quarter.QuarterNumber} {qs.Quarter.Year}",
                    baseScore = qs.BaseScore,
                    leaderAverageScore = qs.LeaderAverageScore,
                    managementBonusScore = qs.ManagementBonusScore,
                    managementBonusReason = qs.ManagementBonusReason,
                    totalScore = qs.TotalScore,
                    evaluationId = qs.EvaluationId,
                    createdAt = qs.CreatedAt,
                    updatedAt = qs.UpdatedAt
                })
                .ToListAsync();

            return Ok(quarterlyScores);
        }
    }
}