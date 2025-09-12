namespace YuGiOh.Domain.Models
{
    public class Match
    {
        public int No { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; }
        public bool IsRunning { get; set; }
        public bool IsFinished { get; set; }
        public DateTime StartDate { get; set; }

        public string WhitePlayerId { get; set; }
        public Account WhitePlayer { get; set; }
        public int WhitePlayerResult { get; set; }

        public string BlackPlayerId { get; set; }
        public Account BlackPlayer { get; set; }
        public int BlackPlayerResult { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}