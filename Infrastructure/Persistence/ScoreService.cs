using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ScoreService
    {
        private readonly IMongoCollection<Score> _score;
        private readonly ScoreDatabaseSettings _scoreDatabaseSettings;
        public ScoreService(ScoreDatabaseSettings settings)
        {
            _scoreDatabaseSettings = settings;
            var client = new MongoClient(_scoreDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(_scoreDatabaseSettings.DatabaseName);
            _score = database.GetCollection<Score>(_scoreDatabaseSettings.ScoreCollectionName);
        }

        public List<Score> GetTopScores(string ScoreBoardIdentifier, int amount) => _score.Find(
            score => score.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier))
            .SortByDescending(x => x.ScoreAmount)
            .Limit(amount)
            .ToList();

        public List<Score> GetAll() =>
            _score.Find(score => true).ToList();

        public Score GetById(string id) =>
            _score.Find<Score>(score => score.Id == id).FirstOrDefault();

        public Score GetByScoreBoard(string ScoreBoardName) =>
           _score.Find<Score>(score => score.ScoreBoardIdentifier.Equals(ScoreBoardName)).FirstOrDefault();

        public async Task<List<Score>> GetByUserNameAndBoard(string ScoreBoardIdentifier, string Username)
        {
            return await _score.Find<Score>(score =>
                    score.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier) &&
                    score.PlayerName.Equals(Username)).ToListAsync();
        }

        public async Task<bool> DeleteByUserNameAndBoard(string ScoreBoardIdentifier, string Username)
        {
            var result = await _score.DeleteManyAsync<Score>(score =>
                    score.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier) &&
                    score.PlayerName.Equals(Username));
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteByBoardName(string ScoreBoardIdentifier)
        {
            var result = await _score.DeleteManyAsync<Score>(score =>
                    score.ScoreBoardIdentifier.Equals(ScoreBoardIdentifier));
            return result.IsAcknowledged;
        }

        public Score Create(Score score)
        {
            _score.InsertOne(score);
            return score;
        }

        public void Update(string id, Score score) =>
            _score.ReplaceOne(score => score.Id == id, score);

        public void Remove(Score scoreIn) =>
            _score.DeleteOne(score => score.Id == scoreIn.Id);

        public void Remove(string id) =>
            _score.DeleteOne(score => score.Id == id);
    }
}
