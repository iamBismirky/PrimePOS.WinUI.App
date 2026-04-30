using PrimePOS.API.Config;
using PrimePOS.API.Middleware;
using QuestPDF.Infrastructure;

namespace PrimePOS.API;

public class Program
{
    public static void Main(string[] args)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var builder = WebApplication.CreateBuilder(args);

        // 🔧 SERVICES (todo centralizado aquí)
        builder.Services.AddPrimePOS(builder.Configuration);

        builder.Services.AddControllers();

        // 📘 Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(); // 👈 importante si usas UI

        var app = builder.Build();

        // ⚠️ Middleware de errores PRIMERO
        app.UseMiddleware<ExceptionMiddleware>();

        // 🔥 Swagger solo en dev
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // 🔐 AUTH PIPELINE (orden obligatorio)
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseStaticFiles();

        app.Run();
    }
}