using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YuGiOh.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            // Campos de dirección
            builder.Property(a => a.StreetName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(a => a.BuildingName)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(a => a.Apartment)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(a => a.CountryIso2)
                   .IsRequired();

            builder.Property(a => a.StateIso2)
                   .IsRequired();

            builder.Property(a => a.CityIso2)
                   .IsRequired();

            builder.HasOne(a => a.StreetType)
                   .WithMany()
                   .HasForeignKey(a => a.StreetTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

       //      // Índices opcionales (útiles para búsquedas por ubicación)
       //      builder.HasIndex(a => new { a.CountryId, a.StateId, a.CityId });
        }
    }
}
