using System.Collections.Generic;

namespace Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public ICollection<ScoreBoard> ScoreBoards { get; set; }
        public PromoCode PromoCode { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
