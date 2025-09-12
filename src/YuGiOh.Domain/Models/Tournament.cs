namespace YuGiOh.Domain.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RegistrationLimit { get; set; }

        public IEnumerable<Round> Rounds { get; set; }
        public IEnumerable<Sponsorship> SponsoredBy { get; set; }
        public IEnumerable<Registration> Registrations { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}