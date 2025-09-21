using YuGiOh.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YuGiOh.Infrastructure.Persistence.Configurations
{
    public class StreetTypeConfiguration : IEntityTypeConfiguration<StreetType>
    {
        public void Configure(EntityTypeBuilder<StreetType> builder)
        {
            builder.ToTable("StreetTypes");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}
