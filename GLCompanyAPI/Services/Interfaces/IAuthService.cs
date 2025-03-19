using GLCompanyAPI.Models;

namespace GLCompanyAPI.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<bool> RegisterUserAsync(string username, string password);
    }
}