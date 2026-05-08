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

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await WriteResponseAsync(
                context,
                ex.Message,
                ex.StatusCode);
        }
        catch (Exception)
        {
            // Aquí luego puedes agregar logging

            await WriteResponseAsync(
                context,
                "Ha ocurrido un error interno.",
                StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        string message,
        int statusCode)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}