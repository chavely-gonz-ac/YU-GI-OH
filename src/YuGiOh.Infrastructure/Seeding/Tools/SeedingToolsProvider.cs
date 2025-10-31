using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YuGiOh.Domain.Helpers;
using YuGiOh.Domain.Models;
using YuGiOh.Infrastructure.Identity;

namespace YuGiOh.Infrastructure.Seeding.Tools
{
    public partial class SeedingToolsProvider
    {
        public readonly SeedTools<IdentityRole> RoleSeedingTools;
        public readonly SeedTools<Staff> StaffSeedingTools;
        public readonly SeedTools<Account> AccountSeedingTools;

        public readonly List<object> SeedingTools;

        private readonly ILoggerFactory _loggerFactory;

        public SeedingToolsProvider(
            RoleManager<IdentityRole> roleManager,
            IRepositoryBase<Staff> staffRepository,
            UserManager<Account> userManager,
            ILoggerFactory loggerFactory
        )
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            RoleSeedingTools = GenerateRoleSeedingTools(roleManager);
            StaffSeedingTools = GenerateStaffSeedingTools(staffRepository);
            AccountSeedingTools = GenerateAccountSeedingTools(userManager);
            SeedingTools = new List<object>()
            {
                RoleSeedingTools,
                StaffSeedingTools,
                AccountSeedingTools
            };
        }

        protected SeedTools<IdentityRole> GenerateRoleSeedingTools(RoleManager<IdentityRole> roleManager)
        {
            return new SeedTools<IdentityRole>(
                async () => (await JsonReaderWrapper.ResolveDataFromJson<string>("IdentityRole")).Select(role => new IdentityRole(role)),
                role => roleManager.RoleExistsAsync(role.Name),
                async roles => { foreach (var role in roles) await roleManager.CreateAsync(role); },
                _loggerFactory.CreateLogger<SeedTools<IdentityRole>>()
            );
        }
        protected SeedTools<Staff> GenerateStaffSeedingTools(IRepositoryBase<Staff> staffRepository)
        {
            return new SeedTools<Staff>(
                async () => await JsonReaderWrapper.ResolveDataFromJson<Staff>(),
                async staff => !(await staffRepository.GetByIdAsync(staff.Id) == null),
                async staffs => { foreach (var staff in staffs) await staffRepository.AddAsync(staff); },
                _loggerFactory.CreateLogger<SeedTools<Staff>>()
                );
        }
        protected SeedTools<Account> GenerateAccountSeedingTools(UserManager<Account> userManager)
        {
            return new SeedTools<Account>(
                async () => (await JsonReaderWrapper.ResolveDataFromJson<AccountDTO>()).Select(accountDTO => MapFromDTO(accountDTO, userManager)),
                async account => !(await userManager.FindByNameAsync(account.UserName) == null),
                async accounts =>
                {
                    foreach (var account in accounts)
                    {
                        await userManager.CreateAsync(account);
                        await userManager.AddToRoleAsync(account, "Admin");
                    }
                },
                _loggerFactory.CreateLogger<SeedTools<Account>>()
            );
        }
        private static Account MapFromDTO(AccountDTO account, UserManager<Account> userManager)
        {
            var newAccount = new Account
            {
                UserName = account.UserName,
                PasswordHash = account.Password,
                Email = account.Email,
                EmailConfirmed = true
            };
            return newAccount;
        }
        public record AccountDTO
        {
            public string UserName { get; set; } = "";
            public string Password { get; set; } = "";
            public string Email { get; set; } = "";
        }
    }
}