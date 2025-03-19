using GLCompanyAPI.Models;

namespace GLCompanyAPI.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company?> GetByIdAsync(int id);
        Task<Company?> GetByIsinAsync(string isin);
        Task AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task DeleteAsync(int id);
        Task<bool> IsinExistsAsync(string isin);
    }
}