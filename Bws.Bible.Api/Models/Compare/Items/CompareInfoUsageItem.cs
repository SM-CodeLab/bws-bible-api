namespace Bws.Bible.Api.Models.Compare.Items;

/// <summary>
/// Utilisation de l'API "/Compare"
/// </summary>
public class CompareInfoUsageItem
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="apiUrl">Url du contrôleur</param>
    public CompareInfoUsageItem(string apiUrl)
    {
        CompareBibles = apiUrl + "/{bibleA}/{bibleB}";
        CompareBooks = apiUrl + "/{bibleA}/{bibleB}/{book}";
        CompareChapters = apiUrl + "/{bibleA}/{bibleB}/{book}/{chapter}";
        CompareVerses = apiUrl + "/{bibleA}/{bibleB}/{book}/{chapter}:{firstVerse}-{lastVerse}";
        CompareVerse = apiUrl + "/{bibleA}/{bibleB}/{book}/{chapter}:{verse}";
    }

    /// <summary>
    /// Comparer deux Bibles
    /// </summary>
    public string CompareBibles { get; private set; }

    /// <summary>
    /// Comparer un livre dans deux Bibles
    /// </summary>
    public string CompareBooks { get; private set; }

    /// <summary>
    /// Comparer un chapitre dans deux Bibles
    /// </summary>
    public string CompareChapters { get; private set; }

    /// <summary>
    /// Comparer un groupe de versets dans deux Bibles
    /// </summary>
    public string CompareVerses { get; private set; }

    /// <summary>
    /// Comparer un verset dans deux Bibles
    /// </summary>
    public string CompareVerse { get; private set; }
}