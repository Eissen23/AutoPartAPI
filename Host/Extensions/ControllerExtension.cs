using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;


namespace Host.Extensions;

public static class ControllerExtension
{
    public static ApiResult<T> ApiOk<T>(this ControllerBase controller, T data, string message = "Action success")
    {
        return ApiResult<T>.Success(data, message);
    }


    public static ApiResult ApiOk(this ControllerBase controller, string message = "Action success")
    {
        return ApiResult.Success(message);
    }
}
