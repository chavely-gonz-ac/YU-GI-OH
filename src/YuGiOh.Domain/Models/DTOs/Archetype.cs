namespace YuGiOh.Domain.Models.DTOs
{
    public class Archetype
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Deck> Decks { get; set; }
    }
}