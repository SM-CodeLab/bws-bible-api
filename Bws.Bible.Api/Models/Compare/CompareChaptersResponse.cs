using Bws.Bible.Api.Models.Compare.Items;
using System.Collections.Generic;

namespace Bws.Bible.Api.Models.Compare;

public class CompareChaptersResponse : BaseApiResponse
{
    public string IdBibleA { get; set; }

    public string IdBibleB { get; set; }

    public byte IdBook { get; set; }

    public byte IdChapter { get; set; }

    public string BookTitle { get; set; }

    public ChapterComparisonItem ChapterComparison { get; set; }

    public List<InterlinearVerseItem> Verses { get; set; }
}
