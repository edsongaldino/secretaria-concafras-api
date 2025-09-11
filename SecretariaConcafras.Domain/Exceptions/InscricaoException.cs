using System.Net;

namespace SecretariaConcafras.Domain.Exceptions;

public sealed class InscricaoException : Exception
{
    public HttpStatusCode Status { get; }
    public IDictionary<string, string[]>? Errors { get; }

    public InscricaoException(HttpStatusCode status, string message, IDictionary<string, string[]>? errors = null)
        : base(message)
    {
        Status = status;
        Errors = errors;
    }
}
