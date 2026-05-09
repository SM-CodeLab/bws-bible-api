using Bws.Bible.Api.Models.Common;
using Bws.Bible.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bws.Bible.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            throw exception;
        }

        var statusCode = MapStatusCode(exception);

        var error = new ErrorResponse
        {
            Code = statusCode == (int)HttpStatusCode.InternalServerError
                ? "InternalServerError"
                : "Error",
            Message = MapMessage(exception),
            Details = exception.Message,
            StackTrace = _env.IsDevelopment() ? exception.StackTrace : null,
            RequestId = context.TraceIdentifier
        };

        var payload = JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(payload);
    }

    private static string MapMessage(Exception exception)
    {
        return exception switch
        {
            BadHttpRequestException => "Bad request : Invalid parameters detected",
            InfrastructureException => "Service unavailable : An error occurred while accessing the data source",
            _ => "An unexpected error occurred"
        };
    }

    private static int MapStatusCode(Exception exception)
    {
        return exception switch
        {
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            InfrastructureException => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}