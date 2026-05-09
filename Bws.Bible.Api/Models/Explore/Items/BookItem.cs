namespace Bws.Bible.Api.Models.Explore.Items
{
    public class BookItem
    {
        public byte IdBook { get; set; }

        public string Name { get; set; }

        public byte CountChapters { get; set; }

        public short CountVerses { get; set; }

        public string GoToFirstChapter { get; set; }
    }
}