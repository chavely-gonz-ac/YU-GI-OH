namespace YuGiOh.Domain.Models
{
    public class Sponsorship
    {
        public string SponsorId { get; set; }
        public Account Sponsor { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}