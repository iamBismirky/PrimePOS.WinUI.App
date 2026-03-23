using global::PrimePOS.BLL.Services;
using global::PrimePOS.DAL.Context;
using global::PrimePOS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace PrimePOS.WinUI.Infrastructure
{


    namespace PrimePOS.WinUI.Infrastructure
    {
        public static class DependencyInjection
        {
            // Método de extensión para agregar todos los servicios y repositorios
            public static IServiceCollection AddPrimePOSServices(this IServiceCollection services, string connectionString)
            {
                // 1️⃣ DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // 2️⃣ Repositories
                services.AddTransient<UsuarioRepository>();
                services.AddTransient<RolRepository>();
                services.AddTransient<CategoriaRepository>();
                services.AddTransient<ClienteRepository>();
                services.AddTransient<ProductoRepository>();
                services.AddTransient<VentaRepository>();
                services.AddTransient<MetodoPagoRepository>();
                services.AddTransient<CajaRepository>();
                services.AddTransient<TurnoRepository>();

                // 3️⃣ Services
                services.AddTransient<UsuarioService>();
                services.AddTransient<RolService>();
                services.AddTransient<CategoriaService>();
                services.AddTransient<ClienteService>();
                services.AddTransient<ProductoService>();
                services.AddTransient<VentaService>();
                services.AddTransient<MetodoPagoService>();
                services.AddTransient<CajaService>();
                services.AddTransient<TurnoService>();

                return services;
            }
        }
    }
}
