using System;
using System.Collections.Generic;
using System.Linq;
using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Domain.Enums;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Infrastructure.Mapper
{
    public static class BibleMapper
    {
        public static BibleDto ToBibleDto(this BibleData bibleData, bool withVerses = false)
        {
            var bibleDto = new BibleDto();
            bibleDto.Id = bibleData.Id;
            bibleDto.Name = bibleData.Name;
            ELanguage language = ELanguage.Unknown;
            Enum.TryParse(bibleData.Language, true, out language);
            bibleDto.Language = language;
            bibleDto.Translator = bibleData.Translator;
            bibleDto.ReleaseYear = bibleData.ReleaseYear;
            bibleDto.CountBooks = bibleData.Statistics.CountBooks;
            bibleDto.CountVerses = bibleData.Statistics.CountVerses;

            if (bibleData.Verses != null && bibleData.Verses.Any())
            {
                bibleDto.CountVerses = (short)bibleData.Verses.Count;
                bibleDto.Books.AddRange(bibleData.Books.ToBookDto(bibleData, withVerses));
            }
            else
            {
                bibleDto.Books.AddRange(bibleData.Books.ToBookDto());
            }

            return bibleDto;
        }

        public static IEnumerable<BibleDto> ToBibleDto(this IEnumerable<BibleData> biblesData)
        {
            return biblesData.Select(b => b.ToBibleDto());
        }
    }
}