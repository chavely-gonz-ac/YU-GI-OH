namespace YuGiOh.Domain.Models
{
    public class Player
    {
        public required string Id { get; set; }
        public Address? Address { get; set; }
        public int AddressId { get; set; }
    }
}