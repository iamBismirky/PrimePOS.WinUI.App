using Microsoft.EntityFrameworkCore;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<AperturaCaja> AperturaCajas { get; set; }
        public DbSet<MetodoPago> MetodoPagos { get; set; }


        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData
             (
                new Rol { RolId = 1, Nombre = "Desarrollador" },
                new Rol { RolId = 2, Nombre = "Administrador" },
                new Rol { RolId = 3, Nombre = "Cajero" }
             );
            modelBuilder.Entity<MetodoPago>().HasData
             (
                new MetodoPago { MetodoPagoId = 1, Nombre = "Efectivo", Estado = true },
                new MetodoPago { MetodoPagoId = 2, Nombre = "Tarjeta", Estado = true },
                new MetodoPago { MetodoPagoId = 3, Nombre = "Teansferencia", Estado = true }
             );
            modelBuilder.Entity<Caja>().HasData
             (
                new Caja { CajaId = 1, Nombre = "Caja Principal", Estado = true }

             );
            modelBuilder.Entity<Cliente>().HasData
             (
                new Cliente { ClienteId = 1, Nombre = "Consumidor Final", Estado = true }

             );


        }      


    }
}
