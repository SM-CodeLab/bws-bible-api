using Bws.Bible.Core.Configuration;

namespace Bws.Bible.Api.Models.Search.Items
{
    /// <summary>
    /// Paramètres de l'API "/Search"
    /// </summary>
    public class SearchInfoSettingsItem
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="settings">Paramètres de l'API</param>
        public SearchInfoSettingsItem(IApiSettings settings)
        {
            SearchWordsMinimumLength = settings.SearchWordsMinLength;
            SearchWordsMaximumLength = settings.SearchWordsMaxLength;
            SearchPaginationEnabled = settings.SearchPaginationEnabled ? (bool?)true : null;
            SearchPaginationNumberOfVersesPerPage = settings.SearchPaginationEnabled ? (short?)settings.SearchPaginationVersesPerPage : null;
        }

        /// <summary>
        /// Nombre de caractères minimum d'une recherche
        /// </summary>
        public short SearchWordsMinimumLength { get; private set; }

        /// <summary>
        /// Nombre de caractères maximum d'une recherche
        /// </summary>
        public short SearchWordsMaximumLength { get; private set; }

        /// <summary>
        /// Pagination des résultats de recherche activée
        /// </summary>
        public bool? SearchPaginationEnabled { get; private set; }

        /// <summary>
        /// Nombre de versets par page dans les résultats de recherche
        /// </summary>
        public short? SearchPaginationNumberOfVersesPerPage { get; private set; }
    }
}