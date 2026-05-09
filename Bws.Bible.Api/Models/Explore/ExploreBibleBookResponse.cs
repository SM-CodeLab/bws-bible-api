using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Models.Explore
{
    public class ExploreBibleBookResponse : BaseApiResponse
    {
        public string IdBible { get; set; }

        public BookItem Book { get; set; }

        public BookNavigationItem Navigation { get; set; }
    }
}