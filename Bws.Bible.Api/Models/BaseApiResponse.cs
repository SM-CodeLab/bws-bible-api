using Microsoft.AspNetCore.Mvc;

namespace Bws.Bible.Api.Models;

/// <summary>
/// Classe mère des modèles de réponse de l'API Bible
/// </summary>
public abstract class BaseApiResponse
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public BaseApiResponse()
    {
        
    }

    /// <summary>
    /// Transforme la réponse en résultat Json pour l'API
    /// </summary>
    /// <returns>JsonResult</returns>
    public virtual JsonResult ToJsonResult()
    {
        return new JsonResult(this);
    }
}