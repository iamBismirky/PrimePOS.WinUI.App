using PrimePOS.API.Middleware;

namespace PrimePOS.API.Extensions;

public static class AppExtensions
{
    public static WebApplication UseAppPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();

        return app;
    }
}