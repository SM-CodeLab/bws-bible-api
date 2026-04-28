using System.Collections.Generic;

namespace Bws.Bible.Api.Models.Explore.Items
{
    public class ChapterItem
    {
        public byte IdChapter { get; set; }

        public byte CountVerses { get; set; }

        public List<VerseItem> Verses { get; set; }
    }
}