using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YuGiOh.Domain.Models;

namespace YuGiOh.Infrastructure.Persistence.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .IsRequired()
                   .HasMaxLength(50);

            // Relaciones
            builder.HasOne(p => p.Address)
                   .WithMany()
                   .HasForeignKey(p => p.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            // builder.HasMany(p => p.Decks)
            //        .WithOne(d => d.Owner)
            //        .HasForeignKey(d => d.OwnerId)
            //        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
