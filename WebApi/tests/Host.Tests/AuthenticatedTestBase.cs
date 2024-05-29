using FSH.WebApi.Shared.Multitenancy;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Text.Json;

namespace Host.Tests;

public abstract class AuthenticatedTestBase : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime {
    protected readonly HttpClient Client;
    protected AuthenticatedTestBase(TestWebApplicationFactory factory) {
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Add("tenant", "root");
    }

    protected async Task AuthenticateAsAdmin() {
        // Arrange
        var response = await Client.PostAsync("api/tokens", new StringContent($"{{\"Email\":\"{MultitenancyConstants.Root.EmailAddress}\",\"Password\":\"{MultitenancyConstants.DefaultPassword}\"}}", Encoding.UTF8, "application/json"));
        using var tokenResponse = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("tenant", "root");
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.RootElement.GetProperty("token").GetString()}");
    }

    public async Task InitializeAsync() {
        await Task.Run(AuthenticateAsAdmin);
    }

    public Task DisposeAsync() {
        return Task.CompletedTask;
    }
}