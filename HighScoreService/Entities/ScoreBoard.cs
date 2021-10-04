using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ScoreBoard
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string Identifier { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
