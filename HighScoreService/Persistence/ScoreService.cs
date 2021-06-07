using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ScoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScoreService> _logger;

        public ScoreService(ApplicationDbContext context, ILogger<ScoreService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Score> GetTopScores(string ScoreBoardIdentifier, int amount)
        {
            return _context.Scores
                .Where(x => x.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier))
                .OrderBy(x => x.ScoreAmount)
                .Take(amount).AsNoTracking().ToList();
        }
            

        public bool DeleteByUserNameAndBoard(string ScoreBoardIdentifier, string Username)
        {
            try
            {
                var scoresToDelete = _context.Scores.Where(x => x.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier) && x.PlayerName.Equals(Username));
                _context.Scores.RemoveRange(scoresToDelete);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to DeleteByUserNameAndBoard");
                return false;
            }
        }

        public bool DeleteByBoardName(string ScoreBoardIdentifier)
        {
            try
            {
                var scoresToDelete = _context.Scores.Where(x => x.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier));
                _context.Scores.RemoveRange(scoresToDelete);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to DeleteByBoardName");
                return false;
            }
        }

        public Score Create(Score score)
        {
            _context.Scores.Add(score);
            _context.SaveChanges();
            return score;
        }

    }
}
