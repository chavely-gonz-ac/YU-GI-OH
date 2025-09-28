/* src/Domain/DTOs/RegisterUserRequest.cs */

using YuGiOh.Domain.Models;

namespace YuGiOh.Domain.DTOs
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string FullName 
        { 
            get
            { 
                if (string.IsNullOrWhiteSpace(MiddleName))
                    return string.Join(" ", FirstName, FirstSurname, SecondSurname);
                else
                    return string.Join(" ", FirstName, MiddleName, FirstSurname, SecondSurname); 
            }
        }

        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

        public Address? Address { get; set; }

        public string? IBAN { get; set; }
    }
}