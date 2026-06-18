using System;
using System.Text.Json.Serialization;

namespace Shared.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Meta { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success") => new()
    {
        IsSuccess = true,
        Message = message,
        Data = data,
        StatusCode = 200
    };

    public static ApiResponse<T> Success(T data, string message, int statusCode) => new()
    {
        IsSuccess = true,
        Message = message,
        Data = data,
        StatusCode = statusCode
    };

    public static ApiResponse<T> Success(string message = "Success") => new()
    {
        IsSuccess = true,
        Message = message,
        Data = default,
        StatusCode = 200
    };

    public static ApiResponse<T> Success(string message, int statusCode) => new()
    {
        IsSuccess = true,
        Message = message,
        Data = default,
        StatusCode = statusCode
    };


    public static ApiResponse<T> Failure(string message, int statusCode = 400) => new()
    {
        IsSuccess = false,
        Message = message,
        Data = default,
        StatusCode = statusCode
    };
}


/// <summary>
/// For non generic type
/// </summary>
public class ApiResponse : ApiResponse<object>
{

    public static new ApiResponse Success(object data, string message = "Success")
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            StatusCode = 200,
            Timestamp = DateTime.UtcNow
        };
    }

    public static new ApiResponse Success(object data, string message, int statusCode)
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }

    public new static ApiResponse Success(string message = "Success") => new()
    {
        IsSuccess = true,
        Message = message,
        Data = null,
        StatusCode = 200
    };

    public new static ApiResponse Success(string message, int statusCode) => new()
    {
        IsSuccess = true,
        Message = message,
        Data = null,
        StatusCode = statusCode
    };

    public new static ApiResponse Failure(string message, int statusCode = 400) => new()
    {
        IsSuccess = false,
        Message = message,
        Data = null,
        StatusCode = statusCode
    };
}
