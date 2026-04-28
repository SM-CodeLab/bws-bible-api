using System;
using Bws.Bible.Core.Exceptions.Enums;

namespace Bws.Bible.Core.Exceptions
{
    public class InfrastructureException : Exception
    {
        public EInfrastructureErrorCode ErrorCode;

        public string ErrorMessage;
    }
}
