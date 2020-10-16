using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Score
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ScoreBoardName")]
        [BsonRequired]
        public string ScoreBoardName { get; set; }

        [BsonElement("PlayerName")]
        [BsonRequired]
        public string PlayerName { get; set; }

        [BsonElement("ScoreAmount")]
        [BsonRequired]
        public double ScoreAmount { get; set; }

        [BsonElement("TimeAmount")]
        public double TimeAmount { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }
    }
}
