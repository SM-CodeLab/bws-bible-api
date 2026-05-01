using Bws.Bible.Api.Models.Compare.Items;

namespace Bws.Bible.Api.Models.Compare;

public class CompareBiblesResponse : BaseApiResponse
{
    public string IdBibleA { get; set; }

    public string IdBibleB { get; set; }

    public BibleComparisonItem BibleComparison { get; set; }
}