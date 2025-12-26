public class AppSettings
{
    public string DefaultConnection { get; set; }
    public JwtSettings Jwt { get; set; }
    public TmdbSettings TMDB { get; set; }
    public GoogleBooksSettings GoogleBooks { get; set; }
}

public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
}

public class TmdbSettings
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
}

public class GoogleBooksSettings
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
}
