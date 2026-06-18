using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Host.Extensions;

public static class ControllerExtension
{
    public static IActionResult ApiOk<T>(this ControllerBase controller, T data, string message = "Action success")
    {
        var response = ApiResponse<object>.Success(data!, message, StatusCodes.Status200OK);
        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    public static IActionResult ApiOk(this ControllerBase controller, string message = "Action success")
    {
        var response = ApiResponse.Success(message, StatusCodes.Status200OK);
        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    public static IActionResult ApiCreated<T>(this ControllerBase controller, T data, string message = "Action success")
    {
        var response = ApiResponse<object>.Success(data!, message, StatusCodes.Status201Created);
        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}
