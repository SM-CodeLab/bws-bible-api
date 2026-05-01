namespace Bws.Bible.Core.Domain;

/// <summary>
/// Livre (Data Transfer Object)
/// </summary>
public record BookDto
{
    /// <summary>
    /// Identifiant du livre de la Bible
    /// </summary>
    public byte IdBook { get; init; }

    /// <summary>
    /// Titre du livre
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Versets du livre
    /// </summary>
    public List<VerseDto> Verses { get; init; } = new();

    /// <summary>
    /// Nombre de chapitres dans ce livre
    /// </summary>
    public byte CountChapters { get; init; }

    /// <summary>
    /// Nombre total de versets dans ce livre
    /// </summary>
    public short CountVerses { get; init; }
}
