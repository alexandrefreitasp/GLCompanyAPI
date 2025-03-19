using System.Net.Http.Json;
using GLCompanyClient.Models;

namespace GLCompanyClient.Services;

public class CompanyService(HttpClient httpClient)
{
    public async Task<(List<Company>?, string)> GetCompaniesAsync()
    {
        try
        {
            var companies = await httpClient.GetFromJsonAsync<List<Company>>("company");
            return (companies, "");
        }
        catch (Exception ex)
        {
            return (null,ex.Message);
        }
    }

    public async Task<(Company?, string)> GetCompanyById(int id)
    {
        try
        {
            var company = await httpClient.GetFromJsonAsync<Company>($"company/{id}");
            return (company, "");
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public async Task<(Company?, string)> GetCompanyByIsin(string isin)
    {
        try
        {
            var company = await httpClient.GetFromJsonAsync<Company>($"company/{isin}");
            return (company, "");
        }
        catch (Exception ex)
        {
            return (null,  ex.Message);
        }
    }

    public async Task<string> AddCompanyAsync(Company company)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("company", company);
            return response.IsSuccessStatusCode ? "Company added successfully!" : $"Error: {await response.Content.ReadAsStringAsync()}";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> UpdateCompanyAsync(int id, Company? company)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"company/{id}", company);
            return response.IsSuccessStatusCode ? "Company updated successfully!" : $"Error: {await response.Content.ReadAsStringAsync()}";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> DeleteCompanyAsync(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"company/{id}");
            return response.IsSuccessStatusCode ? "Company deleted successfully!" : $"Error: {await response.Content.ReadAsStringAsync()}";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}