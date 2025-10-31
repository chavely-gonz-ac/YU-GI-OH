using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YuGiOh.Domain.DataToObject;
using YuGiOh.Domain.Exceptions;
using YuGiOh.Domain.Services;
using YuGiOh.Infrastructure.Persistence;

namespace YuGiOh.Infrastructure.Identity.Services
{
    public class RegisterHandler : IRegisterHandler
    {
        #region constructor
        private readonly YuGiOhDbContext _dbContext;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterHandler> _logger;

        public RegisterHandler(
            YuGiOhDbContext dbContext,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<RegisterHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Register
        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request == null)
                throw APIException.BadRequest("Argument Null Exception", nameof(request));
            Account? account = null;

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                account = await CreateAccountAsync(request);
                await AddRolesAsync(account, request.Roles);
                await transaction.CommitAsync();
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                _logger.LogError(exception, "User registration failed for email {Email}", request.Email);
                throw;
            }
        }
        private async Task<Account> CreateAccountAsync(RegisterRequest request)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(request.Email))
                throw APIException.BadRequest("Argument Exception", $"Email is required.\n{nameof(request.Email)}");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw APIException.BadRequest("Argument Exception", $"Password is required.\n{nameof(request.Password)}");

            // Check if user already exists
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                throw APIException.BadRequest
                ("Invalid Operation Exception",
                $"An account with email {request.Email} already exists.");


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
                throw APIException.BadRequest("Invalid Operation Exception", $"Could not create account: {errors}");
            }

            return account;
        }
        private async Task AddRolesAsync(Account account, IEnumerable<string> roles)
        {
            if (roles == null)
                throw APIException.BadRequest("Argument Exception", $"Roles are required.\n{nameof(roles)}");

            var toAdd = new List<string>();

            foreach (var roleName in roles.Distinct())
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    throw APIException.BadRequest
                    ("Invalid Operation Exception",
                    $"Role does not exist: {roleName}.");

                toAdd.Add(roleName);
            }

            if (toAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(account, toAdd);
                if (!addResult.Succeeded)
                {
                    var errors = string.Join("; ", addResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to assign roles to user {Email}: {Errors}", account.Email, errors);
                    throw APIException.BadRequest("Invalid Operation Exception", "Not all roles could be assigned to the user.");
                }
            }
            else
                throw APIException.BadRequest("Argument Exception", $"At least one roles is required.");
        }
        #endregion

        #region Confirm Email
        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(email))
                throw APIException.BadRequest("Argument Exception", $"Email is required.\n{nameof(email)}");
            if (string.IsNullOrWhiteSpace(token))
                throw APIException.BadRequest("Argument Exception", $"Token is required.\n{nameof(token)}");
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