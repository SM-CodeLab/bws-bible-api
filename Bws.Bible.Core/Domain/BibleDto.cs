using System.Collections.Generic;
using Bws.Bible.Core.Domain.Enums;

namespace Bws.Bible.Core.Domain
{
    /// <summary>
    /// Bible (Data Transfer Object)
    /// </summary>
    public class BibleDto
    {
        /// <summary>
        /// Identifiant de la Bible
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nom ou titre de la Bible
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Langue
        /// </summary>
        public ELanguage Language { get; set; }

        /// <summary>
        /// Traducteur (personne physique)
        /// </summary>
        public string Translator { get; set; }

        /// <summary>
        /// Date de parution, de publication
        /// </summary>
        public short ReleaseYear { get; set; }

        /// <summary>
        /// Livres de la Bible
        /// </summary>
        public List<BookDto> Books { get; set; }

        /// <summary>
        /// Nombre de livres total dans cette version de la Bible
        /// </summary>
        public short CountBooks { get; set; }

        /// <summary>
        /// Nombre de versets total dans cette version de la Bible
        /// </summary>
        public short CountVerses { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public BibleDto()
        {
            Books = new List<BookDto>();
        }
    }
}
