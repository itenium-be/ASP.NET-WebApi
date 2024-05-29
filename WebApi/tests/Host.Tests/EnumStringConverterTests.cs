using FluentAssertions;
using System.Net;

namespace Host.Tests;

public class ModelBinderTests : AuthenticatedTestBase {
    public ModelBinderTests(TestWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task DeleteMany_MultipleQueryParameterValues_TransformsToGuidArray() {
        // Act
        var response = await Client.DeleteAsync("api/v1.0/Brands?ids=3fa85f64-5717-4562-b3fc-2c963f66afa6, e047ec1b-a059-4624-be5f-7cd48260f1be");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Be("[\"3fa85f64-5717-4562-b3fc-2c963f66afa6\", \"e047ec1b-a059-4624-be5f-7cd48260f1be\"]");
    }
}