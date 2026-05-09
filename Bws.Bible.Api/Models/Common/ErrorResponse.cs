using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bws.Bible.Api.Models.Common;

public class ErrorResponse : BaseApiResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }
    public string StackTrace { get; set; }
    public string RequestId { get; set; }

    public override JsonResult ToJsonResult()
    {
        return new JsonResult(this) { StatusCode = (int)HttpStatusCode.InternalServerError };
    }
}