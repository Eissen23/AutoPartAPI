using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.Common.Exceptions;

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

public class InternalServerException : BaseApiException
{
    public InternalServerException()
        : base(HttpStatusCode.InternalServerError, "Internal Server Error", "")
    {
    }

    public InternalServerException(string message)
        : base(HttpStatusCode.InternalServerError, "Internal Server Error", message)
    {
    }
}

public class NotFoundException : BaseApiException
{
    public NotFoundException()
        : base(HttpStatusCode.NotFound, "NOT_FOUND", "")
    {
    }
    public NotFoundException(string message)
        : base(HttpStatusCode.NotFound, "NOT_FOUND", message)
    {
    }
}

public class ConflicException : BaseApiException
{
    public ConflicException()
        : base(HttpStatusCode.Conflict, "CONFLICT_ERROR", "")
    {
    }
    public ConflicException(string message)
        : base(HttpStatusCode.Conflict, "CONFLICT_ERROR", message)
    {
    }
}


public class ValidationException : BaseApiException
{
    public List<string> Errors { get; set; } = new List<string>();

    public ValidationException()
        : base(HttpStatusCode.BadRequest, "VALIDATION_ERROR", "")
    {
    }
    public ValidationException(string message, List<string>? errors)
        : base(HttpStatusCode.BadRequest, "VALIDATION_ERROR", message)
    {
        Errors = errors is not null ? [.. errors] : [];
    }
}