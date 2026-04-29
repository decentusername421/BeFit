using befit.Data;
using befit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace befit.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var sessions = await _context.Session
                .Where(s => s.UserId == userId)
                .ToListAsync();

            var exercises = await _context.Exercise
                .Include(e => e.Session)
                .Where(e => e.Session!.UserId == userId)
                .ToListAsync();

            var model = new StatisticsViewModel
            {
                TotalSessions = sessions.Count,
                TotalExercises = exercises.Count,
                TotalSeries = exercises.Sum(e => e.NumOfSeries),
                TotalReps = exercises.Sum(e => e.NumOfReps)
            };

            return View(model);
        }
    }
}