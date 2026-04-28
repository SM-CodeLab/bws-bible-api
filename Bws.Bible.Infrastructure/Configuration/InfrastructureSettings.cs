using Bws.Bible.Core.Configuration;

namespace Bws.Bible.Infrastructure.Configuration
{
    public class InfrastructureSettings : IInfrastructureSettings
    {
        public string StoragePath { get; set; }
        public string BibleFileExtension { get; set; }
        public string DelimiterSeparatedValues { get; set; }
        public bool LoadBiblesAtStartup { get; set; }
    }
}
