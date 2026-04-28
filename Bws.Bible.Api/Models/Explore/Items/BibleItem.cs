using System.Collections.Generic;

namespace Bws.Bible.Api.Models.Explore.Items
{
    public class BibleItem
    {
        public string IdBible { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public string Translator { get; set; }

        public short ReleaseYear { get; set; }

        public short CountVerses { get; set; }

        public short CountBooks { get; set; }

        public List<BookInfoItem> Books { get; set; }
    }
}