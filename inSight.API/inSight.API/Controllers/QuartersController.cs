using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;
using inSight.API.DTOs;
using inSight.API.Models;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuartersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuartersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/quarters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuarterDto>>> GetQuarters()
        {
            var quarters = await _context.Quarters
                .OrderByDescending(q => q.Year)
                .ThenByDescending(q => q.QuarterNumber)
                .Select(q => new QuarterDto
                {
                    Id = q.Id,
                    Year = q.Year,
                    QuarterNumber = q.QuarterNumber,
                    StartDate = q.StartDate,
                    EndDate = q.EndDate,
                    IsActive = q.IsActive,
                    IsLocked = q.IsLocked
                })
                .ToListAsync();

            return Ok(quarters);
        }

        // GET: api/quarters/active
        [HttpGet("active")]
        public async Task<ActionResult<QuarterDto>> GetActiveQuarter()
        {
            var quarter = await _context.Quarters
                .Where(q => q.IsActive)
                .Select(q => new QuarterDto
                {
                    Id = q.Id,
                    Year = q.Year,
                    QuarterNumber = q.QuarterNumber,
                    StartDate = q.StartDate,
                    EndDate = q.EndDate,
                    IsActive = q.IsActive,
                    IsLocked = q.IsLocked
                })
                .FirstOrDefaultAsync();

            if (quarter == null)
                return NotFound(new { message = "Nema aktivnog kvartala." });

            return Ok(quarter);
        }

        // GET: api/quarters/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<QuarterDto>> GetQuarter(Guid id)
        {
            var quarter = await _context.Quarters
                .Where(q => q.Id == id)
                .Select(q => new QuarterDto
                {
                    Id = q.Id,
                    Year = q.Year,
                    QuarterNumber = q.QuarterNumber,
                    StartDate = q.StartDate,
                    EndDate = q.EndDate,
                    IsActive = q.IsActive,
                    IsLocked = q.IsLocked
                })
                .FirstOrDefaultAsync();

            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            return Ok(quarter);
        }

        // POST: api/quarters
        [HttpPost]
        public async Task<ActionResult<QuarterDto>> CreateQuarter(CreateQuarterDto dto)
        {
            // Provera da li već postoji kvartal za tu godinu i broj
            var exists = await _context.Quarters
                .AnyAsync(q => q.Year == dto.Year && q.QuarterNumber == dto.QuarterNumber);

            if (exists)
                return BadRequest(new { message = $"Kvartal Q{dto.QuarterNumber} {dto.Year} već postoji." });

            var quarter = new Quarter
            {
                Year = dto.Year,
                QuarterNumber = dto.QuarterNumber,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = false,
                IsLocked = false
            };

            _context.Quarters.Add(quarter);
            await _context.SaveChangesAsync();

            var quarterDto = new QuarterDto
            {
                Id = quarter.Id,
                Year = quarter.Year,
                QuarterNumber = quarter.QuarterNumber,
                StartDate = quarter.StartDate,
                EndDate = quarter.EndDate,
                IsActive = quarter.IsActive,
                IsLocked = quarter.IsLocked
            };

            return CreatedAtAction(nameof(GetQuarter), new { id = quarter.Id }, quarterDto);
        }

        // PUT: api/quarters/{id}/activate
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateQuarter(Guid id)
        {
            var quarter = await _context.Quarters.FindAsync(id);
            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            // Deaktiviraj sve ostale kvartale
            var activeQuarters = await _context.Quarters.Where(q => q.IsActive).ToListAsync();
            foreach (var q in activeQuarters)
            {
                q.IsActive = false;
            }

            quarter.IsActive = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kvartal Q{quarter.QuarterNumber} {quarter.Year} je aktiviran." });
        }

        // PUT: api/quarters/{id}/lock
        [HttpPut("{id}/lock")]
        public async Task<IActionResult> LockQuarter(Guid id)
        {
            var quarter = await _context.Quarters.FindAsync(id);
            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            quarter.IsLocked = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kvartal Q{quarter.QuarterNumber} {quarter.Year} je zaključan." });
        }

        // PUT: api/quarters/{id}/unlock
        [HttpPut("{id}/unlock")]
        public async Task<IActionResult> UnlockQuarter(Guid id)
        {
            var quarter = await _context.Quarters.FindAsync(id);
            if (quarter == null)
                return NotFound(new { message = "Kvartal nije pronađen." });

            quarter.IsLocked = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kvartal Q{quarter.QuarterNumber} {quarter.Year} je otključan." });
        }
    }
}