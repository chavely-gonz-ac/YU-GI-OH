using Ardalis.Specification;

using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Models;
using YuGiOh.Domain.Services;
using YuGiOh.Infrastructure.Identity;
using YuGiOh.Infrastructure.Persistence;


namespace YuGiOh.Infrastructure.Identity.Services
{
    public class UserRegistrationHandler : IUserRegistrationHandler
    {
        #region constructor
        private readonly YuGiOhDbContext _dbContext;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepositoryBase<Player> _playerRepository;
        private readonly IRepositoryBase<Sponsor> _sponsorRepository;
        private readonly IRepositoryBase<Address> _addressRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserRegistrationHandler> _logger;

        public UserRegistrationHandler(
            YuGiOhDbContext dbContext,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager,
            IRepositoryBase<Player> playerRepository,
            IRepositoryBase<Sponsor> sponsorRepository,
            IRepositoryBase<Address> addressRepository,
            IConfiguration configuration,
            ILogger<UserRegistrationHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _sponsorRepository = sponsorRepository ?? throw new ArgumentNullException(nameof(sponsorRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region register
        public async Task<string> RegisterAsync(RegisterUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Account? account = null;

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create account
                account = await this.CreateAccountAsync(request);

                // Add roles
                if (request.Roles != null && request.Roles.Any())
                {
                    await this.AddRoles(account, request.Roles);
                }
                else
                {
                    throw new InvalidOperationException("No roles provided for the account.");
                }

                // Create related entities
                await this.CreateRelatedEntitiesAsync(account, request);

                // Commit transaction if everything is OK
                await transaction.CommitAsync();

                return await _userManager.GenerateEmailConfirmationTokenAsync(account);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Delete related entities.

                _logger.LogError(ex, "User registration failed for email {Email}", request.Email);
                throw;
            }
        }

        private async Task<Account> CreateAccountAsync(RegisterUserRequest request)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.", nameof(request.Email));
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.", nameof(request.Password));

            // Check if user already exists
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
            {
                throw new InvalidOperationException($"An account with email {request.Email} already exists.");
            }

            // Create Identity account
            var account = new Account
            {
                UserName = request.FullName,
                Email = request.Email,
            };

            // Save to Identity store
            var createResult = await _userManager.CreateAsync(account, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to create account {Email}: {Errors}", request.Email, errors);
                throw new InvalidOperationException($"Could not create account: {errors}");
            }

            return account;
        }

        private async Task AddRoles(Account account, IEnumerable<string> roles)
        {
            if (roles == null) return;

            var toAdd = new List<string>();

            foreach (var roleName in roles.Distinct())
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    throw new InvalidOperationException($"Role does not exist: {roleName}.");

                toAdd.Add(roleName);
            }

            if (toAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(account, toAdd);
                if (!addResult.Succeeded)
                {
                    var errors = string.Join("; ", addResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to assign roles to user {Email}: {Errors}", account.Email, errors);
                    throw new InvalidOperationException("Not all roles could be assigned to the user.");
                }
            }
        }

        private async Task CreateRelatedEntitiesAsync(Account account, RegisterUserRequest request)
        {
            if (request.Roles.Contains("Player"))
                await this.CreatePlayerAsync(account, request);

            if (request.Roles.Contains("Sponsor"))
                await this.CreateSponsorAsync(account, request);

            if (!request.Roles.Contains("Player") && !request.Roles.Contains("Sponsor"))
                throw new InvalidOperationException("No valid role recognized.");
        }

        private async Task CreatePlayerAsync(Account account, RegisterUserRequest request)
        {
            if (request.Address == null) throw new ArgumentNullException(nameof(request.Address));
            var address = request.Address;
            await _addressRepository.AddAsync(address);

            var player = new Player()
            {
                Id = account.Id,
                AddressId = address.Id,
            };
            await _playerRepository.AddAsync(player);
        }

        private async Task CreateSponsorAsync(Account account, RegisterUserRequest request)
        {
            if (request.IBAN == null) throw new ArgumentNullException(nameof(request.IBAN));
            var sponsor = new Sponsor()
            {
                Id = account.Id,
                IBAN = request.IBAN,
            };
            await _sponsorRepository.AddAsync(sponsor);
        }
        #endregion

        #region confirmation
        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(nameof(email));
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException(nameof(token));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("ConfirmEmail: email not found {Email}", email);
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                _logger.LogWarning("ConfirmEmail failed for {Email}: {Errors}", email, string.Join("; ", result.Errors.Select(e => e.Description)));
                return false;
            }

            return true;
        }
        #endregion
    }
}
