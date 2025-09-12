using YuGiOh.Domain.Models.DTOs;

namespace YuGiOh.Domain.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public int MainDeckSize { get; set; }
        public int SideDeckSize { get; set; }
        public int ExtraDeckSize { get; set; }

        public string OwnerId { get; set; }
        public Account Owner { get; set; }
        public int ArchetypeId { get; set; }
        public Archetype Archetype { get; set; }
        public IEnumerable<Registration> Registrations { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}