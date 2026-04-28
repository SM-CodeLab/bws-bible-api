using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Models.Explore
{
    public class ExploreBibleResponse : BaseApiResponse
    {
        public BibleItem Bible { get; set; }

        public BibleNavigationItem Navigation { get; set; }
    }
}