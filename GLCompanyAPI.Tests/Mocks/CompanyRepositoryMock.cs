using GLCompanyAPI.Models;
using GLCompanyAPI.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLCompanyAPI.Repositories.Interfaces;

namespace GLCompanyAPI.Tests.Mocks
{
    public static class CompanyRepositoryMock
    {
        public static Mock<ICompanyRepository> GetCompanyRepository()
        {
            var companies = new List<Company>
            {
                new Company { Id = 1, Name = "Apple Inc.", Exchange = "NASDAQ", Ticker = "AAPL", Isin = "US0378331005" },
                new Company { Id = 2, Name = "Microsoft Corp.", Exchange = "NASDAQ", Ticker = "MSFT", Isin = "US5949181045" }
            };

            var mockRepo = new Mock<ICompanyRepository>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(companies);
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => companies.FirstOrDefault(c => c.Id == id));
            mockRepo.Setup(repo => repo.GetByIsinAsync(It.IsAny<string>()))
                .ReturnsAsync((string isin) => companies.FirstOrDefault(c => c.Isin == isin));
            mockRepo.Setup(repo => repo.IsinExistsAsync(It.IsAny<string>()))
                .ReturnsAsync((string isin) => companies.Any(c => c.Isin == isin));

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Company>())).Callback((Company company) => companies.Add(company));
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Company>())).Callback((Company company) =>
            {
                var existingCompany = companies.FirstOrDefault(c => c.Id == company.Id);
                if (existingCompany != null)
                {
                    existingCompany.Name = company.Name;
                    existingCompany.Exchange = company.Exchange;
                    existingCompany.Ticker = company.Ticker;
                    existingCompany.Isin = company.Isin;
                }
            });

            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Callback((int id) =>
            {
                var companyToRemove = companies.FirstOrDefault(c => c.Id == id);
                if (companyToRemove != null)
                {
                    companies.Remove(companyToRemove);
                }
            });

            return mockRepo;
        }
    }
}
