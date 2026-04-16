using Microsoft.Extensions.DependencyInjection;
using PrimePOS.WinUI.Services.Api;
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

        services.AddScoped<RolApiService>();
        services.AddScoped<CategoriaApiService>();
        services.AddScoped<CajaApiService>();
        services.AddScoped<ClienteApiService>();
        services.AddScoped<ProductoApiService>();

        // ViewModels
        services.AddTransient<RolViewModel>();

        return services;
    }
}