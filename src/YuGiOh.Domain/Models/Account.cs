using Microsoft.AspNetCore.Identity;

using YuGiOh.Domain.Enums;

namespace YuGiOh.Domain.Models
{
    public class Account : IdentityUser
    {
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public AccountType Type { get; set; }
        public AccountStatement Statement { get; set; }

        public IEnumerable<Deck> Decks { get; set; }
        public IEnumerable<Sponsorship> Sponsored { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}