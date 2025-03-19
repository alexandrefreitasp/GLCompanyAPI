using Blazored.LocalStorage;
using GLCompanyClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;
using GLCompanyClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CompanyService>();

builder.Services.AddScoped<HttpClient>(sp =>
{
    var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5043/api/") };
    return httpClient;
});

var host = builder.Build();
await ConfigureHttpClientAsync(host);
await host.RunAsync();

async Task ConfigureHttpClientAsync(WebAssemblyHost host)
{
    var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
    var httpClient = host.Services.GetRequiredService<HttpClient>();
    var authService = host.Services.GetRequiredService<AuthService>();

    var token = await localStorage.GetItemAsStringAsync("token");

    if (string.IsNullOrEmpty(token))
    {
        token = await authService.AutoLoginAsync();
    }

    if (!string.IsNullOrEmpty(token))
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}