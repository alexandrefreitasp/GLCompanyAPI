using GLCompanyAPI.Data;
using GLCompanyAPI.Models;
using GLCompanyAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GLCompanyAPI.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<Company?> GetByIsinAsync(string isin)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Isin == isin);
        }

        public async Task AddAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            var existingCompany = await _context.Companies.FindAsync(company.Id);
            _context.Entry(existingCompany).State = EntityState.Detached; 

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsinExistsAsync(string isin)
        {
            return await _context.Companies.AnyAsync(c => c.Isin == isin);
        }
    }
}