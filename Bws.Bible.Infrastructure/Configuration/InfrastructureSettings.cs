using Bws.Bible.Core.Configuration;

namespace Bws.Bible.Infrastructure.Configuration
{
    public record InfrastructureSettings : IInfrastructureSettings
    {
        public required string StoragePath { get; init; }
        public required string BibleFileExtension { get; init; }
        public required string DelimiterSeparatedValues { get; init; }
        public bool LoadBiblesAtStartup { get; init; }
    }
}
