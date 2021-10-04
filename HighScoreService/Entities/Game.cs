using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(280)")]
        public string Identifier { get; set; }
        public ICollection<ScoreBoard> ScoreBoards { get; set; }
        public PromoCode PromoCode { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
