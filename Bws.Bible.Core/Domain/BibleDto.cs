using Bws.Bible.Core.Domain.Enums;

namespace Bws.Bible.Core.Domain;

/// <summary>
/// Bible (Data Transfer Object)
/// </summary>
public record BibleDto
{
    /// <summary>
    /// Identifiant de la Bible
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Nom ou titre de la Bible
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Langue
    /// </summary>
    public ELanguage Language { get; init; }

    /// <summary>
    /// Traducteur (personne physique)
    /// </summary>
    public required string Translator { get; init; }

    /// <summary>
    /// Date de parution, de publication
    /// </summary>
    public short ReleaseYear { get; init; }

    /// <summary>
    /// Livres de la Bible
    /// </summary>
    public IReadOnlyList<BookDto> Books { get; init; } = new List<BookDto>();

    /// <summary>
    /// Nombre de livres total dans cette version de la Bible
    /// </summary>
    public short CountBooks { get; init; }

    /// <summary>
    /// Nombre de versets total dans cette version de la Bible
    /// </summary>
    public short CountVerses { get; init; }
}
