using System.Collections.ObjectModel;

namespace Bws.Bible.Api.Configuration;

public record CorsSettings
{
    public string[] AllowedOrigins { get; init; }
    public string[] AllowedMethods { get; init; }
    public string[] AllowedHeaders { get; init; }
}