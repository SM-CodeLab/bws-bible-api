namespace Bws.Bible.Core.Configuration
{
    public interface IApiSettings
    {
        string Name { get; init; }
        string Version { get; init; }
        string BaseUrl { get; init; }
        string Environment { get; init; }
        bool GenerateNavigationLinks { get; init; }
        bool GenerateResponseTime { get; init; }
        string RegexBibleIdentifier { get; init; }
        short SearchWordsMinLength { get; init; }
        short SearchWordsMaxLength { get; init; }
        bool SearchPaginationEnabled { get; init; }
        short SearchPaginationVersesPerPage { get; init; }
        bool CompareInterlinearVersesEnabled { get; init; }
    }
}
