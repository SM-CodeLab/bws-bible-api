using Bws.Bible.Core.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bws.Bible.Api.Controllers.Back;

/// <summary>
/// Monitoring de l'API Bible
/// </summary>
[Route("api/monitoring")]
[ApiController]
public class MonitoringController : ControllerBase
{
    private readonly IApiSettings _apiSettings;
    private readonly IInfrastructureSettings _infrastructureSettings;

    public MonitoringController(IApiSettings apiSettings, IInfrastructureSettings infrastructureSettings)
    {
        _apiSettings = apiSettings;
        _infrastructureSettings = infrastructureSettings;
    }

    /// <summary>
    /// Vérifier le bon état de fonctionnement du service
    /// </summary>
    [EnableCors]
    [HttpGet("health")]
    public JsonResult HealthCheck()
    {
        return new JsonResult(new { HealthCheck = "ok" });
    }

    /// <summary>
    /// Vérifier la configuration du service
    /// </summary>
    [HttpGet("config")]
    public JsonResult ApiConfigCheck()
    {
        return new JsonResult(new { 
            ConfigCheck = "ok", 
            ApiSettings = _apiSettings,
            InfrastructureSettings = _infrastructureSettings
        });
    }
}
