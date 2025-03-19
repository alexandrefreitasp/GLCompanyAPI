using GLCompanyAPI.Models;
using GLCompanyAPI.Services;
using GLCompanyAPI.Tests.Mocks;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GLCompanyAPI.Tests.Services
{
    public class CompanyServiceTests
    {
        private readonly CompanyService _companyService;

        public CompanyServiceTests()
        {
            var mockRepo = CompanyRepositoryMock.GetCompanyRepository();
            _companyService = new CompanyService(mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCompanies()
        {
            var result = await _companyService.GetAllAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCompany_WhenIdExists()
        {
            var result = await _companyService.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Apple Inc.", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
        {
            var result = await _companyService.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFalse_WhenIsinAlreadyExists()
        {
            var newCompany = new Company { Id = 3, Name = "Amazon", Exchange = "NASDAQ", Ticker = "AMZN", Isin = "US0378331005" };
            var result = await _companyService.AddAsync(newCompany);
            Assert.False(result);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnTrue_WhenIsinIsUnique()
        {
            var newCompany = new Company { Id = 3, Name = "Amazon", Exchange = "NASDAQ", Ticker = "AMZN", Isin = "US1234567890" };
            var result = await _companyService.AddAsync(newCompany);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenCompanyExists()
        {
            var existingCompany = new Company { Id = 1, Name = "Apple Updated", Exchange = "NASDAQ", Ticker = "AAPL", Isin = "US0378331005" };
            var result = await _companyService.UpdateAsync(existingCompany);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenCompanyDoesNotExist()
        {
            var nonExistentCompany = new Company { Id = 999, Name = "Non Existent", Exchange = "NYSE", Ticker = "XYZ", Isin = "US9999999999" };
            var result = await _companyService.UpdateAsync(nonExistentCompany);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenCompanyExists()
        {
            var result = await _companyService.DeleteAsync(1);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenCompanyDoesNotExist()
        {
            var result = await _companyService.DeleteAsync(999);
            Assert.False(result);
        }
    }
}
