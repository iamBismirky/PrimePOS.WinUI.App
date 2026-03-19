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
            base.OnModelCreating(modelBuilder);

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
                new MetodoPago { MetodoPagoId = 3, Nombre = "Transferencia", Estado = true }
             );
            modelBuilder.Entity<Caja>().HasData
             (
                new Caja { CajaId = 1, Nombre = "Caja Principal", Estado = true }

             );
            modelBuilder.Entity<Cliente>().HasData
             (
                new Cliente { ClienteId = 1, Nombre = "Consumidor Final", Estado = true }

             );


            // =========================
            // 🔹 USUARIO → VENTA
            // =========================
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Ventas)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 USUARIO → APERTURA CAJA
            // =========================
            modelBuilder.Entity<AperturaCaja>()
                .HasOne(a => a.Usuario)
                .WithMany() // 👈 sin colección
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 CAJA → APERTURA CAJA
            // =========================
            modelBuilder.Entity<AperturaCaja>()
                .HasOne(a => a.Caja)
                .WithMany()
                .HasForeignKey(a => a.CajaId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 APERTURA CAJA → VENTA
            // =========================
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.AperturaCaja)
                .WithMany(a => a.Ventas)
                .HasForeignKey(v => v.AperturaCajaId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 CLIENTE → VENTA
            // =========================
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 METODO PAGO → VENTA
            // =========================
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.MetodoPago)
                .WithMany()
                .HasForeignKey(v => v.MetodoPagoId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 VENTA → DETALLE VENTA
            // =========================
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 PRODUCTO → DETALLE VENTA
            // =========================
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // 🔹 USUARIO → ROL
            // =========================
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany()
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.NoAction);


        }


    }
}
