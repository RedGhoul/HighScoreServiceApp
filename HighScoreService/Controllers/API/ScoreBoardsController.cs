using Application.DTO.INPUT;
using Application.DTO.OUTPUT;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighScoreService.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreBoardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ScoreService _scoreService;

        public ScoreBoardsController(
            ApplicationDbContext context,
            ScoreService scoreService)
        {
            _scoreService = scoreService;
            _context = context;
        }

        // GET: api/ScoreBoards/Add
        [HttpGet("Add")]
        public async Task<ActionResult> AddScore(AddScore addScore)
        {
            var scoreBoard = await _context.ScoreBoards.Where(x => x.Name.Equals(addScore.ScoreBoardName)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");

            var score = _scoreService.Create(new Score
            {
                ScoreBoardName = scoreBoard.Name,
                PlayerName = addScore.PlayerName,
                ScoreAmount = addScore.Score,
                TimeAmount = addScore.TimeAmount,
                Description = addScore.Description
            });

            return Ok(new SanitizedScore
            {
                PlayerName = score.PlayerName,
                ScoreAmount = score.ScoreAmount,
                TimeAmount = score.TimeAmount,
                Description = score.Description
            });

        }

        [HttpGet("Delete")]
        public async Task<ActionResult> DeleteScoreBoard(DeleteScore deleteScore)
        {
            var scoreBoard = await _context.ScoreBoards.Where(x => x.Name.Equals(deleteScore.ScoreBoardName)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");
            if (string.IsNullOrEmpty(deleteScore.PlayerName)) return BadRequest("Invalid User Name");
            if (!await _scoreService.DeleteByUserNameAndBoard(scoreBoard.Name, deleteScore.PlayerName)) return BadRequest("Could Not Delete Scores");
            return Ok();
        }

        [HttpGet("DeleteAll")]
        public async Task<ActionResult> DeleteScoreBoard(DeleteAllScores deleteAllScores)
        {
            var scoreBoard = await _context.ScoreBoards.Where(x => x.Name.Equals(deleteAllScores.ScoreBoardName)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");
            if (!await _scoreService.DeleteByBoardName(scoreBoard.Name)) return BadRequest("Could Not Delete Scores");
            return Ok();
        }

        [HttpGet("GetTop")]
        public async Task<ActionResult> GetTopScores(GetTop getTop)
        {
            if (getTop.NumberOfScores == 0)
            {
                return BadRequest("Number of Scores was set to zero");
            }
            if (getTop.NumberOfScores > 100)
            {
                return BadRequest("Number Of Scores was greater than 100");
            }

            var scoreBoard = await _context.ScoreBoards.Where(x => x.Name.Equals(getTop.ScoreBoardName)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");
            var topScores = _scoreService.GetTopScores(scoreBoard.Name, getTop.NumberOfScores);
            List<SanitizedScore> sanitized = new List<SanitizedScore>();

            foreach (var score in topScores)
            {
                sanitized.Add(new SanitizedScore
                {
                    PlayerName = score.PlayerName,
                    ScoreAmount = score.ScoreAmount,
                    TimeAmount = score.TimeAmount,
                    Description = score.Description
                });
            }

            return Ok(sanitized);
        }


    }
}
