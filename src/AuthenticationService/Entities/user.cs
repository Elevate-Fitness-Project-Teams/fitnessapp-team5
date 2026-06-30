using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Entities
{
    public partial class User : Auditable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        ///////////////////////////////

        public bool IsPremium { get; internal set; }
        public bool ProfileCompleted { get; internal set; }

        public bool IsEmailVerified { get; private set; }
        public bool IsActive { get; private set; } = true;

        public bool IsLockedOut { get; private set; }
        public DateTime? LockedUntil { get; private set; }


        //////////////////////////////

        public ICollection<RefreshToken> RefreshTokens { get; set; }  = [];
        public ICollection<LoginAttempt> LoginAttempts { get; set; }  = [];
        public ICollection<OtpCode> OtpCodes { get; set; }  = [];
    }
}
