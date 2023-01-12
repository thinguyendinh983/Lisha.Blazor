namespace Lisha.Blazor.Infrastructure.Common
{
    public record LanguageCode(string Code, string DisplayName, bool IsRTL = false);

    public static class LocalizationConstants
    {
        public static readonly LanguageCode[] SupportedLanguages =
        {
            new("vi-VN", "Tiếng Việt"),
            new("en-US", "English")
        };
    }
}
