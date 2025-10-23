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
                    IsActive = u.IsActive,
                    TeamLeadId = u.TeamId,
                    TeamLeadName = u.Team != null ? u.Team.Name : null
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
                IsActive = user.IsActive,
                TeamLeadId = user.TeamId,
                TeamLeadName = user.Team != null ? user.Team.Name : null,
                TeamMembers = user.Team != null ? user.Team.Members.Select(tm => new UserDto
                {
                    Id = tm.Id,
                    FirstName = tm.FirstName,
                    LastName = tm.LastName,
                    Email = tm.Email,
                    Role = tm.SystemRole != null ? tm.SystemRole.Name : "N/A",
                    Position = tm.Role != null ? tm.Role.Name : "N/A",
                    CurrentRank = tm.CurrentRank != null ? tm.CurrentRank.Code : null,
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
    }
}