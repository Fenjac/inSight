using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inSight.API.Data;

namespace inSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/maintenance/clean-rank-duplicates
        [HttpPost("clean-rank-duplicates")]
        public async Task<ActionResult> CleanRankDuplicates()
        {
            try
            {
                // Get all ranks grouped by Code only
                var allRanks = await _context.Ranks
                    .OrderBy(r => r.OrderIndex)
                    .ThenBy(r => r.Code)
                    .ThenBy(r => r.CreatedAt)
                    .ToListAsync();

                var duplicatesRemoved = 0;
                var ranksToKeep = new HashSet<Guid>();
                var ranksToDelete = new List<Guid>();

                // Group by Code only to find duplicates
                var groups = allRanks.GroupBy(r => r.Code);

                foreach (var group in groups)
                {
                    var ranksInGroup = group.OrderBy(r => r.CreatedAt).ToList();

                    if (ranksInGroup.Count > 1)
                    {
                        // Keep the first one (oldest), delete the rest
                        var keepRank = ranksInGroup.First();
                        ranksToKeep.Add(keepRank.Id);

                        foreach (var duplicateRank in ranksInGroup.Skip(1))
                        {
                            ranksToDelete.Add(duplicateRank.Id);
                            duplicatesRemoved++;
                        }
                    }
                }

                if (duplicatesRemoved > 0)
                {
                    // Update any references to duplicate ranks
                    var usersWithDuplicateRanks = await _context.Users
                        .Where(u => u.CurrentRankId.HasValue && ranksToDelete.Contains(u.CurrentRankId.Value))
                        .ToListAsync();

                    foreach (var user in usersWithDuplicateRanks)
                    {
                        var duplicateRank = allRanks.First(r => r.Id == user.CurrentRankId);
                        var correctRank = allRanks.First(r =>
                            r.Code == duplicateRank.Code &&
                            ranksToKeep.Contains(r.Id));

                        user.CurrentRankId = correctRank.Id;
                    }

                    await _context.SaveChangesAsync();

                    // Update RankScoreThresholds references
                    var thresholdsWithDuplicates = await _context.RankScoreThresholds
                        .Where(rst => ranksToDelete.Contains(rst.RankId))
                        .ToListAsync();

                    foreach (var threshold in thresholdsWithDuplicates)
                    {
                        var duplicateRank = allRanks.First(r => r.Id == threshold.RankId);
                        var correctRank = allRanks.First(r =>
                            r.Code == duplicateRank.Code &&
                            ranksToKeep.Contains(r.Id));

                        threshold.RankId = correctRank.Id;
                    }

                    await _context.SaveChangesAsync();

                    // Delete duplicate ranks
                    var ranksToDeleteEntities = allRanks.Where(r => ranksToDelete.Contains(r.Id)).ToList();
                    _context.Ranks.RemoveRange(ranksToDeleteEntities);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = $"Uspešno uklonjeno {duplicatesRemoved} duplikata.",
                        duplicatesRemoved = duplicatesRemoved,
                        ranksKept = ranksToKeep.Count
                    });
                }
                else
                {
                    return Ok(new { message = "Nema duplikata u Ranks tabeli.", duplicatesRemoved = 0 });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Greška pri brisanju duplikata: {ex.Message}" });
            }
        }

        // GET: api/maintenance/check-rank-duplicates
        [HttpGet("check-rank-duplicates")]
        public async Task<ActionResult> CheckRankDuplicates()
        {
            var allRanks = await _context.Ranks
                .OrderBy(r => r.OrderIndex)
                .ToListAsync();

            var groups = allRanks.GroupBy(r => r.Code);
            var duplicates = groups.Where(g => g.Count() > 1).ToList();

            var duplicateInfo = duplicates.Select(g => new
            {
                Code = g.Key,
                Count = g.Count(),
                Ranks = g.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    OrderIndex = r.OrderIndex,
                    CreatedAt = r.CreatedAt
                }).ToList()
            }).ToList();

            return Ok(new
            {
                totalDuplicateGroups = duplicates.Count,
                totalDuplicateRanks = duplicates.Sum(g => g.Count() - 1),
                details = duplicateInfo
            });
        }
    }
}
