using System.Collections.Generic;

namespace Bws.Bible.Core.Domain
{
    /// <summary>
    /// Livre (Data Transfer Object)
    /// </summary>
    public class BookDto
    {
        /// <summary>
        /// Identifiant du livre de la Bible
        /// </summary>
        public byte IdBook { get; set; }

        /// <summary>
        /// Titre du livre
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Versets du livre
        /// </summary>
        public List<VerseDto> Verses { get; set; }

        /// <summary>
        /// Nombre de chapitres dans ce livre
        /// </summary>
        public byte CountChapters { get; set; }

        /// <summary>
        /// Nombre total de versets dans ce livre
        /// </summary>
        public short CountVerses { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public BookDto()
        {
            Verses = new List<VerseDto>();
        }
    }
}
