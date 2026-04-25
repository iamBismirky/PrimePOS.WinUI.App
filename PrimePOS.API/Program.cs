using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PrimePOS.API.Config;
using PrimePOS.API.Middleware;
using System.Text;

namespace PrimePOS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddPrimePOS(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var key = builder.Configuration["Jwt:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(key!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false, // 🔥 sin expiración por ahora

                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    };
                });

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
