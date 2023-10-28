namespace Infrastructure.Authentication.Settings;

public class JwtSettings
{
    public const string SettingsKey = nameof(JwtSettings);

    public JwtSettings()
    {
        Issuer = string.Empty;
        Audience = string.Empty;
        SecurityKey = string.Empty;
    }

    public string SecurityKey { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int TokenExpirationInMinutes { get; set; }
}