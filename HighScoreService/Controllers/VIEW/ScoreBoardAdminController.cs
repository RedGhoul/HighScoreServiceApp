using Application.ViewModels;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighScoreService.Controllers.VIEW
{
    public class ScoreBoardAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScoreBoardAdminController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ScoreBoards
        public async Task<IActionResult> ViewAllScoreBoards()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var applicationDbContext = _context.ScoreBoards
                .Include(s => s.Game)
                .Where(x => x.Game.ApplicationUserId.Equals(user.Id));
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ScoreBoards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            var scoreBoard = await _context.ScoreBoards
                .Include(s => s.Game)
                .FirstOrDefaultAsync(
                    m => m.Id == id && 
                    m.Game.ApplicationUserId.Equals(user.Id));

            if (scoreBoard == null)
            {
                return NotFound();
            }

            return View(scoreBoard);
        }

        // GET: ScoreBoards/Create
        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {

            CreateScoreBoardViewModel createScoreBoardView = new CreateScoreBoardViewModel();
            List<SelectListItem> list = new List<SelectListItem>();
            var games = await _context.Games.ToListAsync();
            foreach (var game in games)
            {
                list.Add(new SelectListItem
                {
                    Text = game.Name,
                    Value = game.Id.ToString(),
                });
            }
            createScoreBoardView.Games = list;

            return View(createScoreBoardView);
        }

        // POST: ScoreBoards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GameId")] ScoreBoardViewModel scoreBoard)
        {

            if (ModelState.IsValid)
            {
                ScoreBoard score = new ScoreBoard();
                score.Identifier = Guid.NewGuid().ToString();
                score.Name = scoreBoard.Name;
                score.Game = _context.Games.FirstOrDefault(x => x.Id == scoreBoard.GameId);
                _context.Add(score);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ViewAllScoreBoards));
        }

        // GET: ScoreBoards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var scoreBoard = await _context.ScoreBoards
                    .Include(s => s.Game)
                    .FirstOrDefaultAsync(
                        m => m.Id == id &&
                        m.Game.ApplicationUserId.Equals(user.Id));

            if (scoreBoard == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", scoreBoard.GameId);
            return View(scoreBoard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GameId")] ScoreBoard scoreBoard)
        {
            if (id != scoreBoard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scoreBoard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreBoardExists(scoreBoard.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewAllScoreBoards));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", scoreBoard.GameId);
            return View(scoreBoard);
        }

        // GET: ScoreBoards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var scoreBoard = await _context.ScoreBoards
                .Include(s => s.Game)
                .FirstOrDefaultAsync(
                    m => m.Id == id &&
                    m.Game.ApplicationUserId.Equals(user.Id));

            if (scoreBoard == null)
            {
                return NotFound();
            }

            return View(scoreBoard);
        }

        // POST: ScoreBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scoreBoard = await _context.ScoreBoards.FindAsync(id);
            _context.ScoreBoards.Remove(scoreBoard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewAllScoreBoards));
        }

        private bool ScoreBoardExists(int id)
        {
            return _context.ScoreBoards.Any(e => e.Id == id);
        }
    }
}
