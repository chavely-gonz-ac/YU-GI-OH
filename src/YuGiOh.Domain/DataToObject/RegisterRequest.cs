namespace YuGiOh.Domain.DataToObject
{
    /// <summary>
    /// Represents the data transfer object (DTO) used for registering a new user account.
    /// </summary>
    /// <remarks>
    /// This object encapsulates the user's identifying information, login credentials,
    /// and the list of roles to be assigned upon registration.
    /// It is consumed primarily by the <see cref="YuGiOh.Domain.Services.IRegisterHandler"/>
    /// during the registration workflow.
    /// </remarks>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the user's first (given) name.
        /// </summary>
        /// <remarks>
        /// This field is required and represents the primary personal name of the user.
        /// </remarks>
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's middle name (if any).
        /// </summary>
        /// <remarks>
        /// This field is optional. When not provided, the <see cref="FullName"/> property
        /// will omit this value.
        /// </remarks>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the user's first surname (paternal surname in some cultures).
        /// </summary>
        /// <remarks>
        /// This field is required and used to build the <see cref="FullName"/>.
        /// </remarks>
        public required string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the user's second surname (maternal surname in some cultures).
        /// </summary>
        /// <remarks>
        /// This field is required and used to build the <see cref="FullName"/>.
        /// </remarks>
        public required string SecondSurname { get; set; }

        /// <summary>
        /// Gets the user's complete full name, composed dynamically from the
        /// provided name and surname fields.
        /// </summary>
        /// <value>
        /// If <see cref="MiddleName"/> is not provided, returns a concatenation of:
        /// <c>FirstName + FirstSurname + SecondSurname</c>.
        /// Otherwise, returns:
        /// <c>FirstName + MiddleName + FirstSurname + SecondSurname</c>.
        /// </value>
        /// <remarks>
        /// This property is computed on access and not stored.
        /// It guarantees consistent formatting of the user's full name
        /// throughout the system.
        /// </remarks>
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

        /// <summary>
        /// Gets or sets the email address associated with the user's account.
        /// </summary>
        /// <remarks>
        /// This field is required and serves as the primary login identifier.
        /// It must be unique within the system.
        /// </remarks>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password that will be used to secure the user account.
        /// </summary>
        /// <remarks>
        /// This field is required. It should meet password complexity requirements
        /// defined by the system’s Identity configuration.
        /// </remarks>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the collection of roles to assign to the new user account.
        /// </summary>
        /// <value>
        /// A sequence of role names represented as strings.
        /// Defaults to an empty collection if none are provided.
        /// </value>
        /// <remarks>
        /// Roles define the user's permissions and access level within the system.
        /// If no roles are provided, registration will fail, since at least one valid
        /// role is expected by the <see cref="YuGiOh.Infrastructure.Identity.Services.RegisterHandler"/>.
        /// </remarks>
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
