using GLCompanyAPI.Models;
using GLCompanyAPI.Repositories;
using GLCompanyAPI.Repositories.Interfaces;
using GLCompanyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GLCompanyAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository) => _repository = repository;

        public async Task<IEnumerable<Company>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Company?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Company?> GetByIsinAsync(string isin) => await _repository.GetByIsinAsync(isin);

        public async Task<bool> AddAsync(Company company)
        {
            if (await _repository.IsinExistsAsync(company.Isin)) return false;
            await _repository.AddAsync(company);
            return true;
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            if (await _repository.GetByIdAsync(company.Id) == null) return false;
            await _repository.UpdateAsync(company);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await _repository.GetByIdAsync(id) == null) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}