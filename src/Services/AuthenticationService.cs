using Microsoft.AspNetCore.Identity;
using System.Text;
using DataClash.Data;

namespace DataClash.Services
{
    public class AuthenticationService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthenticationService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(User user, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(user, password);
            return hashedPassword;
        }

        public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            user.PasswordHash = hashedPassword;
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
