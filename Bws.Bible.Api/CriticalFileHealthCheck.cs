using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bws.Bible.Api;

public class CriticalFileHealthCheck : IHealthCheck
{
    private readonly string _filePath;

    public CriticalFileHealthCheck(string filePath)
    {
        _filePath = filePath;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _filePath)))
        {
            return Task.FromResult(HealthCheckResult.Healthy($"Fichier '{_filePath}' accessible."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy($"Fichier '{_filePath}' manquant ou inaccessible."));
    }
}