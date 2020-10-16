namespace Domain.Entities
{
    public class ScoreBoard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
