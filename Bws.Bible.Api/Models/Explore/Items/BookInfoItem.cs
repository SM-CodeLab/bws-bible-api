namespace Bws.Bible.Api.Models.Explore.Items
{
    public class BookInfoItem
    {
        public byte IdBook { get; set; }

        public string Name { get; set; }

        public byte CountChapters { get; set; }

        public short CountVerses { get; set; }

        public string GoToBook { get; set; }
    }
}