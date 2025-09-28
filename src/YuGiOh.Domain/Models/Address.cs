/* src/Domain/Models/Address.cs */

namespace YuGiOh.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string CountryIso2 { get; set; }
        public string StateIso2 { get; set; }
        public string CityIso2 { get; set; }
        public int StreetTypeId { get; set; }
        public StreetType? StreetType { get; set; }
        public string StreetName { get; set; }
        public string BuildingName { get; set; }
        public string Apartment { get; set; }
    }
}