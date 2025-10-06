using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YuGiOh.Domain.Models;

namespace YuGiOh.Infrastructure.Configurations
{
    public class RefreshTokenDataConfiguration : IEntityTypeConfiguration<RefreshTokenData>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenData> builder)
        {
            builder.ToTable("RefreshTokens"); // Table name in DB

            builder.HasKey(r => r.Token); // Token as primary key

            builder.Property(r => r.Token)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(r => r.AccountId)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(r => r.CreatedByIp)
                   .IsRequired()
                   .HasMaxLength(45); // For IPv6 compatibility

            builder.Property(r => r.RevokedByIp)
                   .HasMaxLength(45);

            builder.Property(r => r.ReplacedByToken)
                   .HasMaxLength(200);

            builder.Property(r => r.Expires)
                   .IsRequired();

            builder.Property(r => r.Created)
                   .IsRequired();

            builder.Property(r => r.Revoked)
                   .IsRequired(false);
        }
    }
}
