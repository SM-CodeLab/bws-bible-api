using System.Collections.Generic;
using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Models.Explore
{
    public class ExploreBibleVersesResponse : BaseApiResponse
    {
        public string IdBible { get; set; }

        public byte IdBook { get; set; }

        public byte IdChapter { get; set; }

        public List<VerseItem> Verses { get; set; }

        public VerseNavigationItem Navigation { get; set; }
    }
}