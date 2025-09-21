using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YuGiOh.Domain.Enums;
using YuGiOh.Infrastructure.Identity;

namespace YuGiOh.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Enums
            builder.Property(a => a.Statement)
                   .HasConversion<int>()
                   .HasDefaultValue(AccountStatement.Active)
                   .IsRequired();

            // Fechas
            builder.Property(a => a.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
