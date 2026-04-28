using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bws.Bible.Api.Middlewares
{
    public class BwsCorsMiddleware
    {
        private readonly RequestDelegate _next;

        public BwsCorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            context.Response.Headers["Access-Control-Allow-Methods"] = "GET, OPTIONS";

            return _next(context);
        }
    }
}
