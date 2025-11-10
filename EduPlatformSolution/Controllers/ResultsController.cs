using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EduPlatform.Data;
using Microsoft.AspNetCore.Identity;

namespace EduPlatformSolution.Controllers
{
    [Authorize]
    public class ResultsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResultsController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Results
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var results = await _db.Attempts
                .Include(a => a.Quiz)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.FinishedAt)
                .ToListAsync();

            return View(results);
        }

        // GET: /Results/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var attempt = await _db.Attempts
                .Include(a => a.Quiz)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attempt == null)
                return NotFound();

            return View(attempt);
        }
    }
}
