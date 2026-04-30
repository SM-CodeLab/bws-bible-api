using Bws.Bible.Core.Configuration;

namespace Bws.Bible.Api.Configuration;

public record ApiSettings : IApiSettings
{
    public required string Name { get; init; }
    public required string Version { get; init; }
    public required string BaseUrl { get; init; }
    public required string Environment { get; init; }
    public bool GenerateNavigationLinks { get; init; }
    public bool GenerateResponseTime { get; init; }
    public required string RegexBibleIdentifier { get; init; }
    public short SearchWordsMinLength { get; init; }
    public short SearchWordsMaxLength { get; init; }
    public bool SearchPaginationEnabled { get; init; }
    public short SearchPaginationVersesPerPage { get; init; }
    public bool CompareInterlinearVersesEnabled { get; init; }
}
