using Bws.Bible.Core.Configuration;
using Bws.Bible.Api.Models.Compare.Items;

namespace Bws.Bible.Api.Models.Compare;

/// <summary>
/// Information sur l'API "/Compare"
/// </summary>
public class CompareInfoResponse : BaseApiResponse
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="apiUrl">Url du contrôleur</param>
    /// <param name="settings">Paramètres de l'API</param>
    public CompareInfoResponse(string apiUrl, IApiSettings settings)
    {
        Usage = new CompareInfoUsageItem(apiUrl);
        Settings = new CompareInfoSettingsItem(settings);
    }

    /// <summary>
    /// Utilisation de l'API "/Compare"
    /// </summary>
    public CompareInfoUsageItem Usage { get; private set; }

    /// <summary>
    /// Paramètres de l'API "/Compare"
    /// </summary>
    public CompareInfoSettingsItem Settings { get; private set; }
}
