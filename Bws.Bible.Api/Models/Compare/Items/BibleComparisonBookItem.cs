namespace Bws.Bible.Api.Models.Compare.Items
{
    public class BibleComparisonBookItem
    {
        public byte IdBook { get; set; }

        public string BookTitle { get; set; }

        public string CountChapters { get; set; }

        public string CountVerses { get; set; }
    }
}