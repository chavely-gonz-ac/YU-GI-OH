namespace YuGiOh.Domain.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public bool Accepted { get; set; }
        public bool IsWinner { get; set; }
        public bool IsPlaying { get; set; }
        public string Description { get; set; }

        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}