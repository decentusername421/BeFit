using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using befit.Data;
using befit.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace befit.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Session
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .ToListAsync());

        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var session = await _context.Session
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,End")] Session session)

        {
            session.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (session.End <= session.Start)
            {
                ModelState.AddModelError("", "Data zakończenia musi być późniejsza niż rozpoczęcia.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = await _context.Session
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }
            if (session.End <= session.Start)
            {
                ModelState.AddModelError("", "Data zakończenia musi być późniejsza niż rozpoczęcia.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var existingSession = await _context.Session
                        .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

                   if (existingSession == null)
                    {
                        return NotFound();
                    }

                    existingSession.Start = session.Start;
                    existingSession.End = session.End;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = await _context.Session
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = await _context.Session
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (session != null)
            {
                _context.Session.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Session.Any(e => e.Id == id);
        }
    }
}
