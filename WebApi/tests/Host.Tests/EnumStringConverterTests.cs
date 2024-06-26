using FluentAssertions;
using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Host.Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Host.Tests;

public class EnumStringConverterTests : AuthenticatedTestBase {
    public EnumStringConverterTests(TestWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task CreateBrandTypeRequest_ShouldAcceptEnumValueNameInsteadOfEnumValue() {
        // Arrange
        const string request = @"{
            ""Name"": ""Test"",
            ""Type"": ""ThirdPartyBrand""
        }";

        // Act
        var response = await Client.PostAsync("api/v1/Brands", new StringContent(request, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json")));

        // Assert
        var savedBrandId = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());
        string savedBrandJsonResponse = await (await Client.GetAsync($"api/v1.0/Brands/{savedBrandId}")).Content.ReadAsStringAsync();
        var savedBrand = JsonSerializer.Deserialize<BrandDto>(savedBrandJsonResponse,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new DatePickerConverter(),
                    new JsonStringEnumConverter()
                }
            }
        );
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        savedBrand.Type.Should().Be(BrandType.ThirdPartyBrand);
    }
}