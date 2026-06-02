using Microsoft.Extensions.DependencyInjection;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels;
using PrimePOS.WinUI.ViewModels.Overlays;
using PrimePOS.WinUI.ViewModels.Pages;
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
        services.AddHttpClient<BaseApiService>();

        services.AddScoped<RolApiService>();
        services.AddScoped<CategoriaApiService>();
        services.AddScoped<CajaApiService>();
        services.AddScoped<ClienteApiService>();
        services.AddScoped<ProductoApiService>();
        services.AddScoped<UsuarioApiService>();
        services.AddScoped<TurnoApiService>();
        services.AddScoped<FacturaApiService>();
        services.AddScoped<MetodoPagoApiService>();
        services.AddScoped<VentaApiService>();
        services.AddScoped<CatalogApiService>();
        services.AddScoped<DashboardApiService>();

        // ViewModels

        services.AddSingleton<AppSesionViewModel>();

        services.AddTransient<RolViewModel>();
        services.AddTransient<PerfilViewModel>();
        services.AddTransient<CajaViewModel>();
        services.AddTransient<CategoriaViewModel>();
        services.AddTransient<ClienteViewModel>();
        services.AddTransient<CajaViewModel>();
        services.AddTransient<ProductoViewModel>();
        services.AddTransient<UsuarioViewModel>();
        services.AddTransient<VentaViewModel>();
        services.AddTransient<ReporVentaViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddTransient<FacturaViewModel>();

        //Overlays
        services.AddTransient<ClienteOverlayViewModel>();
        services.AddTransient<UsuarioOverlayViewModel>();
        services.AddTransient<ProductoOverlayViewModel>();
        services.AddTransient<CategoriaOverlayViewModel>();
        services.AddTransient<RolOverlayViewModel>();
        services.AddTransient<LoginOverlayViewModel>();
        services.AddTransient<CajaOverlayViewModel>();
        services.AddTransient<DialogOverlayViewModel>();
        services.AddTransient<AbrirTurnoOverlayViewModel>();
        services.AddTransient<CerrarTurnoOverlayViewModel>();
        services.AddTransient<CobrarOverlayViewModel>();




        //Services
        services.AddSingleton<NotificationService>();
        services.AddSingleton<PdfViewService>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<OverlayService>();
        services.AddSingleton<AuthOverlayService>();

        return services;
    }
}