using System.Collections.Generic;
using System.Linq;
using Bws.Bible.Core.Domain;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Infrastructure.Mapper
{
    public static class VerseMapper
    {
        public static VerseDto ToVerseDto(this VerseData verseData)
        {
            var verseDto = new VerseDto()
            {
                IdBook = verseData.IdBook,
                IdChapter = verseData.IdChapter,
                IdVerse = verseData.IdVerse,
                Text = verseData.Text
            };
            return verseDto;
        }

        public static IEnumerable<VerseDto> ToVerseDto(this IEnumerable<VerseData> versesData)
        {
            return versesData.Select(v => v.ToVerseDto());
        }
    }
}