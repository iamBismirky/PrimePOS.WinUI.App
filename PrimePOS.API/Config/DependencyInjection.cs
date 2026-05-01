using Microsoft.EntityFrameworkCore;
using PrimePOS.API.Extensions;
using PrimePOS.API.Security;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Services;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.DAL.Repositories;
using PrimePOS.DAL.UnitOfWork;

namespace PrimePOS.API.Config
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddPrimePOS(this IServiceCollection services, IConfiguration config)
        {
            // 🔹 DbContext
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                config.GetConnectionString("DefaultConnection")));

            // 🔹 Repositories
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<ICajaRepository, CajaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IMetodoPagoRepository, MetodoPagoRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            services.AddScoped<IFacturaRepository, FacturaRepository>();
            services.AddScoped<ITurnoRepository, TurnoRepository>();
            services.AddScoped<ICierreTurnoRepository, CierreTurnoRepository>();
            services.AddScoped<IDetalleRepository, DetalleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            // 🔹 Services
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICajaService, CajaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMetodoPagoService, MetodoPagoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<ITurnoService, TurnoService>();

            //jwt
            services.AddSingleton<JwtHelper>();

            // 🔐 AUTH + SWAGGER + EXTRA CONFIG
            services.AddJwtAuthentication(config);
            services.AddSwaggerConfig();
            return services;
        }
    }
}
