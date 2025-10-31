namespace YuGiOh.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public required string CountryIso2 { get; set; }
        public string? StateIso2 { get; set; }
        public string? City { get; set; }
        public StreetType? StreetType { get; set; }
        public int StreetTypeId { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingName { get; set; }
        public string? Apartment { get; set; }
    }
}