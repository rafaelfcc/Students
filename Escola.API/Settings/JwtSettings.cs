namespace Escola.API.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = "escola";
        public string Audience { get; set; } = "clients";
        public int ExpireMinutes { get; set; } = 60;
    }
}
