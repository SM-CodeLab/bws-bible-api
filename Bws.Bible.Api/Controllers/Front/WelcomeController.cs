using Microsoft.AspNetCore.Mvc;

namespace Bws.Bible.Api.Controllers.Front;

/// <summary>
/// Accueil
/// </summary>
[Route("")]
[ApiController]
public class WelcomeController : ControllerBase
{

    /// <summary>
    /// Afficher le message de bienvenue
    /// </summary>
    [HttpGet("/")]
    [HttpGet("/index.html")]
    public ActionResult Index()
    {
        return new OkResult();
    }
}
