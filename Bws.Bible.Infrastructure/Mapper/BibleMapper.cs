using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Domain.Enums;
using Bws.Bible.Infrastructure.Data;
using System.Collections.ObjectModel;

namespace Bws.Bible.Infrastructure.Mapper;

public static class BibleMapper
{
    public static BibleDto ToBibleDto(this BibleData bibleData, bool withVerses = false)
    {
        ELanguage language = ELanguage.Unknown;
        Enum.TryParse(bibleData.Language, true, out language);

        short countVerses;
        ReadOnlyCollection<BookDto> books;

        if (bibleData.Verses != null && bibleData.Verses.Any())
        {
            countVerses = (short)bibleData.Verses.Count;
            books = bibleData.Books.ToBookDto(bibleData, withVerses).ToList().AsReadOnly();
        }
        else
        {
            countVerses = bibleData.Statistics.CountVerses;
            books = bibleData.Books.ToBookDto().ToList().AsReadOnly();
        }

        var bibleDto = new BibleDto()
        {
            Id = bibleData.Id,
            Name = bibleData.Name,
            Language = language,
            Translator = bibleData.Translator,
            ReleaseYear = bibleData.ReleaseYear,
            CountBooks = bibleData.Statistics.CountBooks,
            CountVerses = countVerses,
            Books = books
        };

        return bibleDto;
    }

    public static IEnumerable<BibleDto> ToBibleDto(this IEnumerable<BibleData> biblesData)
    {
        return biblesData.Select(b => b.ToBibleDto());
    }
}