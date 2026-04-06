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
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<CierreTurno> CierresTurno { get; set; }
        public DbSet<MetodoPago> MetodoPagos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentasDetalle { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturasDetalle { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Usuario → Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.NoAction);

            // Turno → Usuario
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.Turnos)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // Turno → Caja
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Caja)
                .WithMany()
                .HasForeignKey(t => t.CajaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Venta → Usuario
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Ventas)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // Venta → Cliente
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);

            // Venta → MetodoPago
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.MetodoPago)
                .WithMany()
                .HasForeignKey(v => v.MetodoPagoId)
                .OnDelete(DeleteBehavior.NoAction);

            // Venta → Turno
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Turno)
                .WithMany(t => t.Ventas)
                .HasForeignKey(v => v.TurnoId)
                .OnDelete(DeleteBehavior.NoAction);

            // Detalle → Venta
            modelBuilder.Entity<VentaDetalle>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Detalle → Producto
            modelBuilder.Entity<VentaDetalle>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CierreTurno>()
                .HasOne(c => c.Turno)
                .WithMany(p => p.CierresTurno)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Factura>()
            .HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

            // Relación Factura -> Usuario
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Facturas)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(d => d.Factura)
                .WithMany(f => f.Detalles)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rol>().HasData
             (
                new Rol { RolId = 1, Nombre = "Desarrollador", Estado = true },
                new Rol { RolId = 2, Nombre = "Administrador", Estado = true }
             );
            modelBuilder.Entity<MetodoPago>().HasData
             (
                new MetodoPago { MetodoPagoId = 1, Nombre = "Efectivo", Estado = true },
                new MetodoPago { MetodoPagoId = 2, Nombre = "Tarjeta", Estado = true },
                new MetodoPago { MetodoPagoId = 3, Nombre = "Transferencia", Estado = true }
             );
            modelBuilder.Entity<Caja>().HasData
             (
                new Caja { CajaId = 1, Nombre = "Principal", Estado = true }

             );
            modelBuilder.Entity<Cliente>().HasData
             (
                new Cliente { ClienteId = 1, Codigo = "CLIENT-0001", Nombre = "Consumidor Final", Estado = true }

             );

        }


    }
}
