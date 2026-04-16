using global::PrimePOS.BLL.Interfaces;
using global::PrimePOS.BLL.Services;
using global::PrimePOS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;

namespace PrimePOS.API
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

            // 🔹 Services
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICajaService, CajaService>();

            return services;
        }
    }
}
