using PrimePOS.BLL.Exceptions;
using PrimePOS.Contracts.Common;
using System.Text.Json;

namespace PrimePOS.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await WriteResponse(context, ex.Message, ex.StatusCode);
        }
        catch (Exception ex)
        {
            await WriteResponse(context, ex.Message, 500);
        }
    }

    private static async Task WriteResponse(HttpContext context, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

}