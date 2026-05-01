using Bws.Bible.Core.Domain;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Infrastructure.Mapper;

public static class BookMapper
{
    public static BookDto ToBookDto(this BookData bookData)
    {
        var bookDto = new BookDto()
        {
            IdBook = bookData.IdBook,
            Title = bookData.Title,
            CountChapters = bookData.Statistics.CountChapters,
            CountVerses = bookData.Statistics.CountVerses
        };
        return bookDto;
    }

    public static IEnumerable<BookDto> ToBookDto(this IEnumerable<BookData> booksData)
    {
        return booksData.Select(b => b.ToBookDto());
    }


    public static BookDto ToBookDto(this BookData bookData, BibleData bibleData, bool withVerses = false)
    {
        IEnumerable<VerseData> verses = new List<VerseData>();
        if (bibleData != null && withVerses)
        {
            verses = bibleData.Verses.Where(v => v.IdBook == bookData.IdBook);
        }

        var bookDto = new BookDto()
        {
            IdBook = bookData.IdBook,
            Title = bookData.Title,
            CountChapters = bookData.Statistics.CountChapters,
            CountVerses = bookData.Statistics.CountVerses,
            Verses = verses.ToVerseDto().ToList()
        };
        
        return bookDto;
    }

    public static IEnumerable<BookDto> ToBookDto(this IEnumerable<BookData> booksData, BibleData bibleData, bool withVerses = false)
    {
        return booksData.Select(b => b.ToBookDto(bibleData, withVerses));
    }
}