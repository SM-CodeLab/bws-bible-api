using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Models.Explore
{
    public class ExploreBibleChapterResponse : BaseApiResponse
    {
        public string IdBible { get; set; }

        public byte IdBook { get; set; }

        public ChapterItem Chapter { get; set; }

        public ChapterNavigationItem Navigation { get; set; }
    }
}