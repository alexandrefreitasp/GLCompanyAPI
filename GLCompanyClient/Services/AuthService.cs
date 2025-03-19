using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", new { Username = username, PasswordHash = password });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result?.Token != null)
            {
                await _localStorage.SetItemAsStringAsync("token", result.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
                return true;
            }
        }
        return false;
    }

    public async Task<string?> AutoLoginAsync()
    {
        var appSecret = "hP9x$2@jJzQ7!vL3eF8&dYkT6*BmNcR4";
        var response = await _httpClient.PostAsJsonAsync("auth/auto-login", appSecret);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result?.Token != null)
            {
                await _localStorage.SetItemAsStringAsync("token", result.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
                return result.Token;
            }
        }
        return null;
    }
}

public class AuthResponse
{
    public string Token { get; set; }
}
