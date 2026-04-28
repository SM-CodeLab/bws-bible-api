namespace Bws.Bible.Core.Configuration
{
    public interface IInfrastructureSettings
    {
        string StoragePath { get; set; }
        string BibleFileExtension { get; set; }
        string DelimiterSeparatedValues { get; set; }
        bool LoadBiblesAtStartup { get; set; }
    }
}
