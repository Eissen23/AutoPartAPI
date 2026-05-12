
using System.Net;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shared.Common;

namespace Host.Extensions;

public class ApiResult<T> : IActionResult
{
    private readonly T _data;
    private readonly string _message;
    private readonly HttpStatusCode _status;

    public ApiResult(T data, string message = "Successfully", HttpStatusCode status = HttpStatusCode.OK)
    {
        _data = data;
        _message = message;
        _status = status;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var apiResponse = ApiResponse<T>.Success(_data, _message);
        var objectResult = new ObjectResult(apiResponse)
        {
            StatusCode = (int)_status
        };

        await objectResult.ExecuteResultAsync(context);
    }

    public static ApiResult<T> Success(T data, string message = "Action success")
    {
        return new ApiResult<T>(data, message);
    }

    public static ApiResult<T> Success(T data, string message, HttpStatusCode statusCode)
    {
        return new ApiResult<T>(data, message, statusCode);
    }
}

public class ApiResult : IActionResult
{
    private readonly string _message;
    private readonly int _statusCode;

    public ApiResult(string message = "Action success", int statusCode = 200)
    {
        _message = message;
        _statusCode = statusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var apiResponse = ApiResponse.Success(_message);

        var objectResult = new ObjectResult(apiResponse)
        {
            StatusCode = _statusCode
        };

        await objectResult.ExecuteResultAsync(context);
    }

    public static ApiResult Success(string message = "Action success")
    {
        return new ApiResult(message);
    }
}