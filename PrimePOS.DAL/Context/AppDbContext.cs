using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Seeds;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Caja> Cajas => Set<Caja>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<MetodoPago> MetodoPagos => Set<MetodoPago>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaDetalle> VentasDetalle => Set<VentaDetalle>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<FacturaDetalle> FacturasDetalle => Set<FacturaDetalle>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<TipoCliente> TipoClientes => Set<TipoCliente>();
    public DbSet<TipoVenta> TipoVentas => Set<TipoVenta>();
    public DbSet<EstadoVenta> EstadoVentas => Set<EstadoVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplySeedData();
    }
}
