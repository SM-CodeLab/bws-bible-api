using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bws.Bible.Api.Middlewares;

public class ChronoMiddleware
{
    private readonly RequestDelegate _next;

    public ChronoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
            context.Response.Headers["X-Response-Time-ms"] = elapsedMs.ToString("0.##");
            return Task.CompletedTask;
        });

        await _next(context);
    }
}