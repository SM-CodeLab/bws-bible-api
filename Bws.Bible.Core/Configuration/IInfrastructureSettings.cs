namespace Bws.Bible.Core.Configuration;

public interface IInfrastructureSettings
{
    string StoragePath { get; init; }
    string BibleFileExtension { get; init; }
    string BibleFileForHealthCheck { get; init; }
    string DelimiterSeparatedValues { get; init; }
    bool LoadBiblesAtStartup { get; init; }
}
