using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Host.Common.Exceptions;

public abstract class BaseApiException : System.Exception
{
    /// <summary>
    /// HTTP status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; protected set; }

    /// <summary>
    /// Error code for exception.
    /// </summary>
    public string ErrorCode { get; protected set; } = string.Empty;

    /// <summary>
    /// Create basic API exception.
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="errorCode"></param>
    protected BaseApiException(HttpStatusCode statusCode, string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    protected BaseApiException(HttpStatusCode statusCode, string errorCode, string message, System.Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}
