using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Domain.Enums;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Infrastructure.Mapper
{
    public static class BibleMapper
    {
        public static BibleDto ToBibleDto(this BibleData bibleData, bool withVerses = false)
        {
            ELanguage language = ELanguage.Unknown;
            Enum.TryParse(bibleData.Language, true, out language);

            bool bibleDataHasVerses = bibleData.Verses != null && bibleData.Verses.Any();

            short countVerses = bibleData.Statistics.CountVerses;
            if(bibleDataHasVerses)
                countVerses = (short)bibleData.Verses.Count;

            var books = bibleDataHasVerses 
                ? bibleData.Books.ToBookDto(bibleData, withVerses).ToList().AsReadOnly() 
                : bibleData.Books.ToBookDto().ToList().AsReadOnly();

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
}