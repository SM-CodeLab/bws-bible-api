namespace Bws.Bible.Api.Models.Search.Items
{
    /// <summary>
    /// Utilisation de l'API "/Search"
    /// </summary>
    public class SearchInfoUsageItem
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="apiUrl">Url du contrôleur</param>
        public SearchInfoUsageItem(string apiUrl)
        {
            SearchInBible = apiUrl + "/{bible}/{words}";
            SearchInBook = apiUrl + "/{bible}/{book}/{words}";
            SearchInChapter = apiUrl + "/{bible}/{book}/{chapter}/{words}";
            SearchInVerses = apiUrl + "/{bible}/{book}/{chapter}:{firstVerse}-{lastVerse}/{words}";
            SearchInVerse = apiUrl + "/{bible}/{book}/{chapter}:{verse}/{words}";
        }

        /// <summary>
        /// Rechercher dans la Bible
        /// </summary>
        public string SearchInBible { get; private set; }

        /// <summary>
        /// Rechercher dans un livre de la Bible
        /// </summary>
        public string SearchInBook { get; private set; }

        /// <summary>
        /// Rechercher dans un chapitre
        /// </summary>
        public string SearchInChapter { get; private set; }

        /// <summary>
        /// Rechercher dans des versets
        /// </summary>
        public string SearchInVerses { get; private set; }

        /// <summary>
        /// Rechercher dans un verset
        /// </summary>
        public string SearchInVerse { get; private set; }
    }
}