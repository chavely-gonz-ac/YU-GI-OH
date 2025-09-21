/* src/Domain/Models/Player.cs */

using System.Collections.Generic;

namespace YuGiOh.Domain.Models
{
    public class Player
    {
        public string Id { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        // public ICollection<Deck> Decks { get; set; } = new List<Deck>();
    }
}
