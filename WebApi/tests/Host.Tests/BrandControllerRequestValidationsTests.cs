using FluentAssertions;
using FSH.WebApi.Application.Catalog.Brands;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Host.Tests;

public class BrandControllerRequestValidationsTests : AuthenticatedTestBase {
    public BrandControllerRequestValidationsTests(TestWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task UpdateBrandRequest_QueryIdAndContentMismatch_RaiseValidationError() {
        // Arrange
        var updateBrandRequest = new UpdateBrandRequest {
            Id = Guid.Parse("ca43337c-c9bc-4c18-98e1-3bf8694f9fb7"),
            Name = "Brand Name",
            Description = "Brand Description"
        };
        // Act

        var response = await Client.PutAsync("api/v1/Brands/538f5ce7-9bd4-4309-8e62-6e566b01fdfe", new StringContent(JsonConvert.SerializeObject(updateBrandRequest), Encoding.UTF8, "application/json"));
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errorMessage = JsonDocument.Parse(await response.Content.ReadAsStringAsync())
                                       .RootElement.GetProperty("title").GetString()
                                       .ToLowerInvariant();
        errorMessage.Should().Contain("validation errors");
    }
}