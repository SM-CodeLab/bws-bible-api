using System.Collections.Generic;

namespace Bws.Bible.Api.Models.Compare.Items
{
    public class BibleComparisonItem
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public string Translator { get; set; }

        public string ReleaseYear { get; set; }

        public string CountVerses { get; set; }

        public string CountBooks { get; set; }

        public List<BibleComparisonBookItem> BooksComparison { get; set; }
    }
}
