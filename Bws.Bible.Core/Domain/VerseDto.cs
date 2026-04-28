namespace Bws.Bible.Core.Domain
{
    /// <summary>
    /// Verset (Data Transfer Object)
    /// </summary>
    public class VerseDto
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
        public string Text { get; set; }
    }
}
