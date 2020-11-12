

using Newtonsoft.Json;
using System;

namespace Domain.Entities
{
    public class Score
    {
        public Score()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public string ScoreBoardIdentifier { get; set; }
        public string PlayerName { get; set; }
        public double ScoreAmount { get; set; }
        public double TimeAmount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ScoreBoardId { get; set; }
        [JsonIgnore]
        public ScoreBoard ScoreBoard { get; set; }
    }
}
