using YuGiOh.Domain.Models.DTOs;

namespace YuGiOh.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int StreetTypeId { get; set; }
        public StreetType StreetType { get; set; }
        public string StreetName { get; set; }
        public string BuildingName { get; set; }
        public string Apartment { get; set; }
    }
}