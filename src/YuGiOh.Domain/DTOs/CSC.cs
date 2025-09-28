namespace YuGiOh.Domain.DTOs
{
    public class Country
    {
        public string Iso2 { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Iso2}:{Name}";
        }
    }

    public class State
    {
        public string Iso2 { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string CountryIso2 { get; set; } = null!;

        public override string ToString()
        {
            return $"{CountryIso2}:{Iso2}:{Name}";
        }
    }

    public class City
    {
        public string Name { get; set; } = null!;
        public string StateIso2 { get; set; } = null!;
        public string CountryIso2 { get; set; } = null!;
    }
}
