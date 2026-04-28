namespace Bws.Bible.Api.Models.Search.Items
{
    /// <summary>
    /// Verset trouvé
    /// </summary>
    public class VerseFoundItem
    {
        /// <summary>
        /// Identifiant du livre
        /// </summary>
        public byte IdBook { get; set; }

        /// <summary>
        /// Numéro de chapitre
        /// </summary>
        public byte IdChapter { get; set; }

        /// <summary>
        /// Numéro de verset
        /// </summary>
        public byte IdVerse { get; set; }

        /// <summary>
        /// Verset
        /// </summary>
        public string Verse { get; set; }

        /// <summary>
        /// Nombre de résultats trouvés dans le verset
        /// </summary>
        public byte CountResult { get; set; }
    }
}