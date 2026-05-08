namespace Bws.Bible.Infrastructure.Data;

/// <summary>
/// Verset de la Bible (Data Access Layer Object)
/// </summary>
public class VerseData
{
    /// <summary>
    /// Identifiant du livre
    /// </summary>
    public byte IdBook { get; set; }

    /// <summary>
    /// Numéro du chapitre
    /// </summary>
    public byte IdChapter { get; set; }

    /// <summary>
    /// Numéro du verset
    /// </summary>
    public byte IdVerse { get; set; }

    /// <summary>
    /// Texte du verset
    /// </summary>
    public required string Text { get; set; }
}