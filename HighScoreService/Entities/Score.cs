using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Score
    {
        public Score()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public string Id { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string ScoreBoardIdentifier { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string PlayerName { get; set; }
        public double ScoreAmount { get; set; }
        public double TimeAmount { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ScoreBoardId { get; set; }
        [JsonIgnore]
        public ScoreBoard ScoreBoard { get; set; }
    }
}
