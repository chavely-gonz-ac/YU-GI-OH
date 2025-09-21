/* src/Domain/Models/Sponsor.cs */

using System.Collections.Generic;

namespace YuGiOh.Domain.Models
{
    public class Sponsor
    {
        public string Id { get; set; }
        public string IBAN { get; set; }

        // public ICollection<Sponsorship> Sponsored { get; set; } = new List<Sponsorship>();
    }
}
