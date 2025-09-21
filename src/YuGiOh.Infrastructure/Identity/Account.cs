using Microsoft.AspNetCore.Identity;

using YuGiOh.Domain.Enums;

namespace YuGiOh.Infrastructure.Identity
{
    public class Account : IdentityUser
    {
        public AccountStatement Statement { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
