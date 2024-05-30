
using FluentAssertions;
using FSH.WebApi.Application.Catalog.Products;
using System.Text;
using System.Text.Json;

namespace Host.Tests;

public class ResponseCompressionTests : AuthenticatedTestBase {
    public ResponseCompressionTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetRequest_ResponseIsCompressed()
    {
        // Arrange
        var requestUri = "api/v1/Products/search";
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new SearchProductsRequest
        {
            PageNumber = 1,
            PageSize = 5000
        }), Encoding.UTF8, "application/json");

        // Act
        var uncompressedResponse = await Client.PostAsync(requestUri, requestContent);
        long? uncompressedSize = uncompressedResponse.Content.Headers.ContentLength;

        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        request.Content = requestContent;
        request.Headers.Add("Accept-Encoding", "gzip");
        var compressedResponse = await Client.SendAsync(request);
        long? compressedSize = compressedResponse.Content.Headers.ContentLength;

        // Assert
        uncompressedSize.Should().BeGreaterThan(compressedSize ?? 0L);
    }
}