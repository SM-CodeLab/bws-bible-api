using Bws.Bible.Core.Exceptions;

namespace Bws.Bible.Infrastructure.Data;

/// <summary>
/// Bible (Data Access Layer Object)
/// </summary>
public class BibleData : IDisposable
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Language { get; set; }

    public required string Translator { get; set; }

    public short ReleaseYear { get; set; }

    public List<VerseData> Verses { get; set; }

    public List<BookData> Books { get; set; }

    public ComputedData Statistics { get; set; }

    public class ComputedData
    {
        public byte CountBooks { get; set; }

        public short CountVerses { get; set; }
    }

    public BibleData()
    {
        Books = new List<BookData>();
        Verses = new List<VerseData>();
        Statistics = new ComputedData();
    }

    public void Dispose()
    {
        Books.Clear();
        Verses.Clear();
    }
}
