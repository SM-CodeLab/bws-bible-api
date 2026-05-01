using Bws.Bible.Api.Models.Compare.Items;

namespace Bws.Bible.Api.Models.Compare;

public class CompareBooksResponse : BaseApiResponse
{
    public string IdBibleA { get; set; }

    public string IdBibleB { get; set; }

    public byte IdBook { get; set; }

    public BookComparisonItem BookComparison { get; set; }
}