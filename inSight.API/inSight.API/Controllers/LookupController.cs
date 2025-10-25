using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LookupController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/lookup/systemroles
        [HttpGet("systemroles")]
        public async Task<ActionResult<IEnumerable<object>>> GetSystemRoles()
        {
            var systemRoles = await _context.SystemRoles
                .Select(sr => new
                {
                    id = sr.Id,
                    name = sr.Name,
                    permissionLevel = sr.PermissionLevel
                })
                .OrderBy(sr => sr.permissionLevel)
                .ToListAsync();

            return Ok(systemRoles);
        }

        // GET: api/lookup/roles
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new
                {
                    id = r.Id,
                    name = r.Name
                })
                .OrderBy(r => r.name)
                .ToListAsync();

            return Ok(roles);
        }

        // GET: api/lookup/teams
        [HttpGet("teams")]
        public async Task<ActionResult<IEnumerable<object>>> GetTeams()
        {
            var teams = await _context.Teams
                .Select(t => new
                {
                    id = t.Id,
                    name = t.Name
                })
                .OrderBy(t => t.name)
                .ToListAsync();

            return Ok(teams);
        }

        // GET: api/lookup/ranks
        [HttpGet("ranks")]
        public async Task<ActionResult<IEnumerable<object>>> GetRanks()
        {
            var ranks = await _context.Ranks
                .Select(r => new
                {
                    id = r.Id,
                    code = r.Code,
                    name = r.Name,
                    minSalary = r.MinSalary,
                    maxSalary = r.MaxSalary,
                    orderIndex = r.OrderIndex
                })
                .OrderBy(r => r.orderIndex)
                .ToListAsync();

            return Ok(ranks);
        }

        // NOTE: Removed GetRanksByRole endpoint - ranks are now universal for all roles
    }
}
