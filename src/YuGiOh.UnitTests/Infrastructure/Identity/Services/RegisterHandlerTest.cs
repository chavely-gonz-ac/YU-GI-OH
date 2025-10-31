
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YuGiOh.Domain.DataToObject;
using YuGiOh.Domain.Exceptions;
using YuGiOh.Infrastructure.Identity;
using YuGiOh.Infrastructure.Identity.Services;

namespace YuGiOh.Tests.UnitTests.Infrastructure.Identity.Services
{
    public class RegisterHandlerTests
    {
        private readonly Mock<DbContext> _dbContextMock;
        private readonly Mock<UserManager<Account>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<RegisterHandler>> _loggerMock;
        private readonly Mock<IDbContextTransaction> _transactionMock;

        private readonly RegisterHandler _handler;

        public RegisterHandlerTests()
        {
            var userStore = new Mock<IUserStore<Account>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            _userManagerMock = new Mock<UserManager<Account>>(userStore.Object, null, null, null, null, null, null, null, null);
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<RegisterHandler>>();
            _dbContextMock = new Mock<DbContext>(new DbContextOptions<DbContext>());
            _transactionMock = new Mock<IDbContextTransaction>();

            var dbFacadeMock = new Mock<DatabaseFacade>(_dbContextMock.Object);
            dbFacadeMock.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(_transactionMock.Object);

            _dbContextMock.Setup(d => d.Database).Returns(dbFacadeMock.Object);

            _handler = new RegisterHandler(
                _dbContextMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _configurationMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_Should_Throw_When_Request_Is_Null()
        {
            Func<Task> act = async () => await _handler.RegisterAsync(null!);
            await act.Should().ThrowAsync<APIException>()
                .WithMessage("*Argument Null Exception*");
        }

        [Fact]
        public async Task RegisterAsync_Should_Create_Account_And_Assign_Roles()
        {
            // Arrange
            var request = new RegisterRequest
            {
                FirstName = "Yugi",
                FirstSurname = "Moto",
                SecondSurname = "Gagi",
                Email = "test@example.com",
                Password = "Test123$",
                Roles = new List<string> { "Player" }
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Account?)null);

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<Account>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.AddToRolesAsync(It.IsAny<Account>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.RegisterAsync(request);

            // Assert
            _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<Account>(), "Test123$"), Times.Once);
            _userManagerMock.Verify(u => u.AddToRolesAsync(It.IsAny<Account>(), It.Is<IEnumerable<string>>(r => r.Contains("Player"))), Times.Once);
            _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Rollback_When_Exception_Occurs()
        {
            var request = new RegisterRequest
            {
                FirstName = "Kaiba",
                FirstSurname = "Seto",
                SecondSurname = "Gagi",
                Email = "fail@example.com",
                Password = "123456",
                Roles = new List<string> { "Admin" }
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            Func<Task> act = async () => await _handler.RegisterAsync(request);

            await act.Should().ThrowAsync<Exception>();
            _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Return_False_When_User_Not_Found()
        {
            _userManagerMock.Setup(u => u.FindByEmailAsync("missing@example.com"))
                .ReturnsAsync((Account?)null);

            var result = await _handler.ConfirmEmailAsync("missing@example.com", "token");
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Return_True_When_Success()
        {
            var account = new Account { Email = "duelist@example.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(account.Email))
                .ReturnsAsync(account);
            _userManagerMock.Setup(u => u.ConfirmEmailAsync(account, "token"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _handler.ConfirmEmailAsync(account.Email, "token");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Throw_When_Arguments_Invalid()
        {
            Func<Task> act1 = async () => await _handler.ConfirmEmailAsync("", "token");
            Func<Task> act2 = async () => await _handler.ConfirmEmailAsync("email@example.com", "");

            await act1.Should().ThrowAsync<APIException>();
            await act2.Should().ThrowAsync<APIException>();
        }
    }
}
