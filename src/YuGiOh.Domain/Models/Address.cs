/* src/Domain/Models/Address.cs */

namespace YuGiOh.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int StreetTypeId { get; set; }
        public StreetType StreetType { get; set; }
        public string StreetName { get; set; }
        public string BuildingName { get; set; }
        public string Apartment { get; set; }
    }
}