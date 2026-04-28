using System.Collections.Generic;
using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Models.Explore
{
    public class ExploreBiblesResponse : BaseApiResponse
    {
        public List<BibleInfoItem> Bibles { get; set; }
    }
}