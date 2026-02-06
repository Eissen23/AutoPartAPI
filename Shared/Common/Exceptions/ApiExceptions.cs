using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Shared.Common.Exceptions;

namespace Shared.Common.Exception;

/// <summary>
/// Exception khi không có quyền truy cập.
/// </summary>
public class ForbiddenException : BaseApiException
{
    public ForbiddenException()
        : base(HttpStatusCode.Forbidden, "FORBIDDEN", "")
    {
    }

    public ForbiddenException(string message)
        : base(HttpStatusCode.Forbidden, "FORBIDDEN",  message)
    {
    }
}

public class UnauthorizedException : BaseApiException
{
    public UnauthorizedException()
        : base(HttpStatusCode.Unauthorized, "UNAUTHORIZED", "")
    {
    }

    public UnauthorizedException(string message)
        : base(HttpStatusCode.Unauthorized, "UNAUTHORIZED", message)
    {
    }
}