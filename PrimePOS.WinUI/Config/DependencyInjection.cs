using Microsoft.Extensions.DependencyInjection;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using System;

namespace PrimePOS.WinUI.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7096/");
        });
        //API Services
        services.AddScoped<RolApiService>();
        services.AddScoped<CategoriaApiService>();
        services.AddScoped<CajaApiService>();
        services.AddScoped<ClienteApiService>();
        services.AddScoped<ProductoApiService>();
        services.AddScoped<UsuarioApiService>();
        services.AddTransient<BaseApiService>();

        // ViewModels
        services.AddTransient<RolViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddSingleton<AppSesionViewModel>();
        services.AddTransient<PerfilViewModel>();
        services.AddScoped<CajaViewModel>();
        services.AddScoped<CategoriaViewModel>();
        services.AddScoped<ClienteViewModel>();
        services.AddScoped<CajaViewModel>();


        //NotificationService
        services.AddSingleton<NotificationService>();
        services.AddSingleton<MainWindow>();

        return services;
    }
}