namespace Bws.Bible.Core.Configuration
{
    public interface IApiSettings
    {
        string Name { get; set; }
        string Version { get; set; }
        string BaseUrl { get; set; }
        string Environment { get; set; }
        bool GenerateNavigationLinks { get; set; }
        bool GenerateResponseTime { get; set; }
        string RegexBibleIdentifier { get; set; }
        short SearchWordsMinLength { get; set; }
        short SearchWordsMaxLength { get; set; }
        bool SearchPaginationEnabled { get; set; }
        short SearchPaginationVersesPerPage { get; set; }
        bool CompareInterlinearVersesEnabled { get; set; }
    }
}
