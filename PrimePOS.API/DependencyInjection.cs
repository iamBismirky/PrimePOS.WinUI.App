//using global::PrimePOS.BLL.Interfaces;
//using global::PrimePOS.BLL.Services;
//using global::PrimePOS.DAL.Interfaces;
//using global::PrimePOS.DAL.Repositories;
//using Microsoft.EntityFrameworkCore;
//using PrimePOS.DAL.Context;

//namespace PrimePOS.API
//{

//    public static class DependencyInjection
//    {
//        public static IServiceCollection AddPrimePOS(this IServiceCollection services, IConfiguration config)
//        {
//            // 🔹 DbContext
//            services.AddDbContext<AppDbContext>(options =>
//                    options.UseSqlServer(
//                config.GetConnectionString("DefaultConnection")));

//            // 🔹 Repositories
//            services.AddScoped<IRolRepository, RolRepository>();

//            // 🔹 Services
//            services.AddScoped<IRolService, RolService>();

//            return services;
//        }
//    }
//}
