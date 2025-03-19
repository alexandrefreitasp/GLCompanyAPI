using GLCompanyAPI.Models;
using GLCompanyAPI.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace GLCompanyAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || user.PasswordHash != HashPassword(password))
                return null;

            return user;
        }

        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            if (await _userRepository.GetByUsernameAsync(username) != null)
                return false;

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}