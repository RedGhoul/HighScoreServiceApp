using Bogus;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HighScoreService.Controllers.VIEW
{
    public class ScoreBoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreBoardController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ScoreBoards/Create
        public async Task<IActionResult> CreateScoreBoard()
        {

            ScoreBoard newScoreBoard = new ScoreBoard()
            {
                Name = new Faker().Company.CompanyName() + " Score Board Anonymous",
                Identifier = Guid.NewGuid().ToString()
            };

            Game newGame = new Game()
            {
                Name = new Faker().Name.FirstName() + " Game Anonymous",
                Identifier = Guid.NewGuid().ToString(),
                ScoreBoards = new List<ScoreBoard>()
            };

            newGame.ScoreBoards.Add(newScoreBoard);

            _context.Games.Add(newGame);

            await _context.SaveChangesAsync();

            return View(newScoreBoard);
        }
    }
}