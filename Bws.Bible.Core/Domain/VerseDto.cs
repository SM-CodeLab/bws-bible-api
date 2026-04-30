namespace Bws.Bible.Core.Domain
{
    /// <summary>
    /// Verset (Data Transfer Object)
    /// </summary>
    public record VerseDto
    {
        /// <summary>
        /// Identifiant du livre
        /// </summary>
        public byte IdBook { get; init; }

        /// <summary>
        /// Numéro du chapitre
        /// </summary>
        public byte IdChapter { get; init; }

        /// <summary>
        /// Numéro du verset
        /// </summary>
        public byte IdVerse { get; init; }

        /// <summary>
        /// Texte du verset
        /// </summary>
        public string Text { get; init; }
    }
}
