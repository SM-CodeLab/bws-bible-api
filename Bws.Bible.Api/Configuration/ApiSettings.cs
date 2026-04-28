using Bws.Bible.Core.Configuration;

namespace Bws.Bible.Api.Configuration
{
    public class ApiSettings : IApiSettings
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string BaseUrl { get; set; }
        public string Environment { get; set; }
        public bool GenerateNavigationLinks { get; set; }
        public bool GenerateResponseTime { get; set; }
        public string RegexBibleIdentifier { get; set; }
        public short SearchWordsMinLength { get; set; }
        public short SearchWordsMaxLength { get; set; }
        public bool SearchPaginationEnabled { get; set; }
        public short SearchPaginationVersesPerPage { get; set; }
        public bool CompareInterlinearVersesEnabled { get; set; }
    }
}
