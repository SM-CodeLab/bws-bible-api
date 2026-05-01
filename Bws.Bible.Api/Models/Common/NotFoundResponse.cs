using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bws.Bible.Api.Models.Common;

public class NotFoundResponse : ErrorResponse
{
    public NotFoundResponse()
    {
        Code = "NotFound";
        Message = "No data found for your request";
    }

    public override JsonResult ToJsonResult()
    {
        return new JsonResult(this) { StatusCode = (int)HttpStatusCode.NotFound };
    }
}
