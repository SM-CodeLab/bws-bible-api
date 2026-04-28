using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Models.Explore
{
    public class ExploreBibleVerseResponse : BaseApiResponse
    {
        public string IdBible { get; set; }

        public byte IdBook { get; set; }

        public byte IdChapter { get; set; }

        public VerseItem Verse { get; set; }

        public VerseNavigationItem Navigation { get; set; }
    }
}