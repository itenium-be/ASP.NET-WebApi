BrandsController :: Create
==========================

3. Enums

Program.cs

```c#
builder.Services.AddControllers().AddJsonOptions(x =>
{
  x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
```



4. Custom Date Format

```c#
builder.Services.AddControllers().AddJsonOptions(x =>
{
  x.JsonSerializerOptions.Converters.Add(new DatePickerConverter());
});
```

```c#
public class DatePickerConverter : JsonConverter<DateTime>
{
  public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? dateString = reader.GetString();
    if (string.IsNullOrEmpty(dateString))
      return DateTime.MinValue;

    if (DateTime.TryParseExact(dateString, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
      return result;

    return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
  }

  public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString("ddMMyyyy"));
  }
}
```


5. Europe vs America

Program.cs

```c#
builder.Services.AddControllers().AddJsonOptions(x =>
{
  // We cannot use DI in the DatePickerConverter because the IOC container has not yet been built
  // This works because of the static field in the HttpContextAccessor
  // private static readonly AsyncLocal<HttpContextHolder> _httpContextCurrent
  // See: https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http/src/HttpContextAccessor.cs
  // Explained: https://blog.stephencleary.com/2016/12/eliding-async-await.html

  // Note that this is NOT best practice!
  var httpContextAccessor = new HttpContextAccessor();
  var languageProvider = new LanguageProvider(httpContextAccessor);
  x.JsonSerializerOptions.Converters.Add(new DatePickerConverter(languageProvider));
});

// Or directly:
// builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILanguageProvider, LanguageProvider>();
```


```c#
public enum Language
{
  European,
  American,
}

public interface ILanguageProvider
{
  Language GetLanguage();
}

public class LanguageProvider : ILanguageProvider
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public LanguageProvider(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Language GetLanguage()
  {
    var context = _httpContextAccessor.HttpContext;
    if (context != null && context.Request.Headers.TryGetValue("Accept-Language", out var acceptLanguage))
    {
      if (acceptLanguage == "en-US")
      {
        return Language.American;
      }
    }
    return Language.European;
  }
}

public class DatePickerConverter : JsonConverter<DateTime>
{
  private readonly ILanguageProvider _langProvider;

  public DatePickerConverter(ILanguageProvider langProvider)
  {
    _langProvider = langProvider;
  }

  public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? dateString = reader.GetString();
    if (string.IsNullOrEmpty(dateString))
        return DateTime.MinValue;

    string format = _langProvider.GetLanguage() == Language.American ? "MMddyyyy" : "ddMMyyyy";
    if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
      return result;

    return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
  }
}
```
