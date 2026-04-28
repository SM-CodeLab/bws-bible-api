using System.Collections.Generic;
using Bws.Bible.Api.Models.Compare.Items;

namespace Bws.Bible.Api.Models.Compare
{
    public class CompareVersesResponse : BaseApiResponse
    {
        public string IdBibleA { get; set; }

        public string IdBibleB { get; set; }

        public byte IdBook { get; set; }

        public byte IdChapter { get; set; }

        public byte IdFirstVerse { get; set; }

        public byte IdLastVerse { get; set; }

        public VerseComparisonItem VersesComparison { get; set; }

        public List<InterlinearVerseItem> Verses { get; set; }
    }
}