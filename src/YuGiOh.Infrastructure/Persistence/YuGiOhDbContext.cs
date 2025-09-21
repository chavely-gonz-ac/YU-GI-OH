using YuGiOh.Domain.Models;
using YuGiOh.Domain.Models.DTOs;
using YuGiOh.Infrastructure.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace YuGiOh.Infrastructure.Persistence
{
    public class YuGiOhDbContext : IdentityDbContext<Account, IdentityRole, string>
    {
        public YuGiOhDbContext(DbContextOptions<YuGiOhDbContext> options) : base(options)
        {
        }

        // Entidades principales
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }

        // Catálogos DTOs
        public DbSet<StreetType> StreetTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Aplica automáticamente todas las configuraciones
            builder.ApplyConfigurationsFromAssembly(typeof(YuGiOhDbContext).Assembly);
        }
    }
}
