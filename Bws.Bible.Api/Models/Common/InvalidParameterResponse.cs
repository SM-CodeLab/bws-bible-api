using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bws.Bible.Api.Models.Common;

public class InvalidParameterResponse : ErrorResponse
{
    public InvalidParameterResponse()
    {
        Code = "InvalidParameters";
        Message = "Bad request : Invalid parameters detected";
    }

    public override JsonResult ToJsonResult()
    {
        return new JsonResult(this) { StatusCode = (int)HttpStatusCode.BadRequest };
    }
}