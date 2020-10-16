namespace Domain.Entities
{
    public class PromoCode
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}