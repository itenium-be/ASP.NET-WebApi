namespace FSH.WebApi.Host.Infrastructure;

public enum Language
{
    European,
    /// <summary>
    /// For Accept-Language "en-US"
    /// </summary>
    American,
}

public interface ILanguageProvider
{
    /// <summary>
    /// Reads the language from the Accept-Language header
    /// </summary>
    Language GetLanguage();
}

public class LanguageProvider : ILanguageProvider
{
    public Language GetLanguage()
    {
        return Language.European;
    }
}