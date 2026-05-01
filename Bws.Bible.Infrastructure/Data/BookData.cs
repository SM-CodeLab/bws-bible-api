namespace Bws.Bible.Infrastructure.Data;

/// <summary>
/// Livre de la Bible (Data Access Layer Object)
/// </summary>
public class BookData
{
    /// <summary>
    /// Identifiant du livre
    /// </summary>
    public byte IdBook { get; set; }

    /// <summary>
    /// Titre du livre
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Données statistiques
    /// </summary>
    public ComputedData Statistics { get; set; }

    public class ComputedData
    {
        public byte CountChapters { get; set; }

        public short CountVerses { get; set; }
    }

    public BookData()
    {
        Statistics = new ComputedData();
    }
}
