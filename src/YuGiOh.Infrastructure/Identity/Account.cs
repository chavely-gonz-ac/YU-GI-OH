using Microsoft.AspNetCore.Identity;

namespace YuGiOh.Infrastructure.Identity
{
    public class Account : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
    }
}
