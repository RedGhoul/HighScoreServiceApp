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
    public class PlayerScoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ScoreService _scoreService;

        public PlayerScoreController(
            ApplicationDbContext context,
            ScoreService scoreService)
        {
            _scoreService = scoreService;
            _context = context;
        }

        // GET: api/ScoreBoards/Add
        [HttpPost("Add")]
        public async Task<ActionResult> AddScore([FromQuery] AddScore addScore)
        {
            var scoreBoard = await _context.ScoreBoards.Where(x => x.Identifier.Equals(addScore.Identifier)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");

            var score = _scoreService.Create(new Score
            {
                ScoreBoardIdentifier = scoreBoard.Identifier,
                PlayerName = addScore.PlayerName,
                ScoreAmount = addScore.Score,
            });

            return Ok(new SanitizedScore
            {
                PlayerName = score.PlayerName,
                ScoreAmount = score.ScoreAmount,
            });

        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteScoreBoard([FromQuery] DeleteScore deleteScore)
        {
            var scoreBoard = await _context.ScoreBoards.Where(x => x.Identifier.Equals(deleteScore.Identifier)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");
            if (string.IsNullOrEmpty(deleteScore.PlayerName)) return BadRequest("Invalid User Name");
            if (!_scoreService.DeleteByUserNameAndBoard(scoreBoard.Identifier, deleteScore.PlayerName)) return BadRequest("Could Not Delete Scores");
            return Ok();
        }

        [HttpPost("DeleteAll")]
        public async Task<ActionResult> DeleteScoreBoard([FromQuery] DeleteAllScores deleteAllScores)
        {
            var scoreBoard = await _context.ScoreBoards.Where(x => x.Identifier.Equals(deleteAllScores.Identifier)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");
            if (!_scoreService.DeleteByBoardName(scoreBoard.Identifier)) return BadRequest("Could Not Delete Scores");
            return Ok();
        }

        [HttpGet("GetTop")]
        public async Task<ActionResult> GetTopScores([FromQuery] GetTop getTop)
        {
            if (getTop.NumberOfScores == 0)
            {
                return BadRequest("Number of Scores was set to zero");
            }
            if (getTop.NumberOfScores > 100)
            {
                return BadRequest("Number Of Scores was greater than 100");
            }

            var scoreBoard = await _context.ScoreBoards.Where(x => x.Identifier.Equals(getTop.Identifier)).FirstOrDefaultAsync();
            if (scoreBoard == null) return BadRequest("Could not find your board");
            var topScores = _scoreService.GetTopScores(scoreBoard.Identifier, getTop.NumberOfScores);
            List<SanitizedScore> sanitized = new List<SanitizedScore>();

            foreach (var score in topScores)
            {
                sanitized.Add(new SanitizedScore
                {
                    PlayerName = score.PlayerName,
                    ScoreAmount = score.ScoreAmount,
                });
            }

            return Ok(sanitized);
        }


    }
}
