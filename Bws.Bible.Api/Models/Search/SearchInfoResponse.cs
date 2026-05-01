using Bws.Bible.Core.Configuration;
using Bws.Bible.Api.Models.Search.Items;

namespace Bws.Bible.Api.Models.Search
{
    /// <summary>
    /// Information sur l'API "/Search"
    /// </summary>
    public class SearchInfoResponse : BaseApiResponse
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="apiUrl">Url du contrôleur</param>
        /// <param name="settings">Paramètres de l'API</param>
        public SearchInfoResponse(string apiUrl, IApiSettings settings)
        {
            Usage = new SearchInfoUsageItem(apiUrl);
            Settings = new SearchInfoSettingsItem(settings);
        }

        /// <summary>
        /// Utilisation de l'API "/Search"
        /// </summary>
        public SearchInfoUsageItem Usage { get; private set; }

        /// <summary>
        /// Paramètres de l'API "/Search"
        /// </summary>
        public SearchInfoSettingsItem Settings { get; private set; }
    }
}
