using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Domain.Exceptions;

public class ExternalGatewayException : Exception
{
    public string Code { get; }
    public int HttpStatus { get; }

    public ExternalGatewayException(string code, string message, int httpStatus = 502, Exception? inner = null)
        : base(message, inner)
    {
        Code = code;
        HttpStatus = httpStatus;
    }
}
