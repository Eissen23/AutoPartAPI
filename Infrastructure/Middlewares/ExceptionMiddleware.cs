using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Shared.Common;
using Shared.Common.Exceptions;

namespace Infrastructure.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BaseApiException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;

            var response = ApiResponse.Failure(ex.Message, (int)ex.StatusCode);
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = ApiResponse.Failure("An unexpected error occurred.", StatusCodes.Status500InternalServerError);
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
