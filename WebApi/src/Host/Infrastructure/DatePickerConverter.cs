using System.Text.Json;
using System.Text.Json.Serialization;

namespace FSH.WebApi.Host.Infrastructure;

/// <summary>
/// Allow a DateTime to also be parsed from format ddMMyyyy
/// </summary>
public class DatePickerConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.MinValue;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        
    }
}