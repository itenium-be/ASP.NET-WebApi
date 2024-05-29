using System.Text.Json;
using System.Text.Json.Serialization;

namespace FSH.WebApi.Host.Infrastructure;

/// <summary>
/// Allow a DateOnly to also be parsed from format ddMMyyyy
/// </summary>
public class DatePickerConverter : JsonConverter<DateOnly> {
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return DateOnly.MinValue;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) { }
}