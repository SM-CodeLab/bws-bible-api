using System.Collections.Generic;

namespace Bws.Bible.Api.Models.Common;

public class InvalidParameterResponse : BaseApiResponse
{
    public List<string> Errors { get; set; }
}