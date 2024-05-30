using FSH.WebApi.Domain.Catalog;

namespace FSH.WebApi.Infrastructure.Catalog;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CustomProductConverter : JsonConverter<Product>
{
    public override Product Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        var name = rootElement.GetProperty("Name").GetString();
        var description = rootElement.GetProperty("Description").GetString();
        var rate = rootElement.GetProperty("Rate").GetDecimal();
        var imagePath = rootElement.GetProperty("ImagePath").GetString();
        var brandId = rootElement.GetProperty("BrandId").GetGuid();

        return new Product(name, description, rate, brandId, imagePath);
    }

    public override void Write(Utf8JsonWriter writer, Product value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("Id", value.Id.ToString());
        writer.WriteString("Name", value.Name);
        writer.WriteString("Description", value.Description);
        writer.WriteNumber("Rate", value.Rate);
        writer.WriteString("ImagePath", value.ImagePath);
        writer.WriteString("BrandId", value.BrandId.ToString());

        writer.WriteEndObject();
    }
}
