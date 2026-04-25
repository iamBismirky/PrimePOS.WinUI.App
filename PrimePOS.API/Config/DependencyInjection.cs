using Microsoft.EntityFrameworkCore;
using PrimePOS.API.Security;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Services;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.DAL.Repositories;

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

            // 🔹 Services
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICajaService, CajaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();


            //jwt
            services.AddSingleton<JwtHelper>();

            return services;
        }
    }
}
