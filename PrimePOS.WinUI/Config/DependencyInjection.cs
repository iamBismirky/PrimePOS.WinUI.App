using Microsoft.Extensions.DependencyInjection;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Net.Http;

namespace PrimePOS.WinUI.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // HttpClient
        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7096/")
        });

        // Services API
        services.AddSingleton<RolApiService>();
        //services.AddSingleton<LoadingService>();

        // ViewModels
        services.AddTransient<RolViewModel>();

        return services;
    }
}