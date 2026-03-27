using global::PrimePOS.BLL.Services;
using global::PrimePOS.DAL.Context;
using global::PrimePOS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.DAL.UnitOfWork;
using PrimePOS.WinUI.ViewModels;
namespace PrimePOS.WinUI.Infrastructure
{


    namespace PrimePOS.WinUI.Infrastructure
    {
        public static class DependencyInjection
        {
            // Método de extensión para agregar todos los servicios y repositorios
            public static IServiceCollection AddPrimePOSServices(this IServiceCollection services, string connectionString)
            {
                //  DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

                //  Repositories
                services.AddScoped<UsuarioRepository>();
                services.AddScoped<RolRepository>();
                services.AddScoped<CategoriaRepository>();
                services.AddScoped<ClienteRepository>();
                services.AddScoped<ProductoRepository>();
                services.AddScoped<VentaRepository>();
                services.AddScoped<MetodoPagoRepository>();
                services.AddScoped<CajaRepository>();
                services.AddScoped<TurnoRepository>();
                services.AddScoped<CierreTurnoRepository>();

                //  Services
                services.AddScoped<UsuarioService>();
                services.AddScoped<RolService>();
                services.AddScoped<CategoriaService>();
                services.AddScoped<ClienteService>();
                services.AddScoped<ProductoService>();
                services.AddScoped<VentaService>();
                services.AddScoped<MetodoPagoService>();
                services.AddScoped<CajaService>();
                services.AddScoped<TurnoService>();

                //UnitOfWork
                services.AddScoped<UnitOfWork>();

                //ViewModel
                services.AddScoped<VentaViewModel>();




                return services;

            }
        }
    }
}
