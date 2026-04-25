using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class VentaRepository : IVentaRepository
{
    private readonly AppDbContext _context;
    public VentaRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Crear(Venta venta)
    {
        _context.Add(venta);
    }
    public void Actualizar(Venta venta)
    {
        _context.Update(venta);
    }
    public void Anular(Venta venta)
    {
        venta.Estado = false;
        _context.Update(venta);
    }
    public async Task<Venta?> ObtenerPorId(int id)
    {
        return _context.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefault(v => v.VentaId == id);
    }
    public List<Venta> ListarVentas()
    {
        return _context.Ventas.Include(v => v.Detalles).ToList();
    }
    public async Task<decimal> ObtenerTotalPorMetodoPagoAsync(int turnoId, int metodoPagoId)
    {
        return await _context.Ventas
            .Where(v => v.TurnoId == turnoId && v.MetodoPagoId == metodoPagoId)
            .SumAsync(v => v.Total);
    }
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<Venta>> ObtenerPorTurnoAsync(int turnoId)
    {
        return await _context.Ventas
            .Where(v => v.TurnoId == turnoId && v.Estado)
            .ToListAsync();
    }

    public async Task<List<Venta>> ObtenerPorFechaAsync(DateTime fecha)
    {
        return await _context.Ventas
            .Where(v => v.FechaRegistro.Date == fecha.Date && v.Estado)
            .ToListAsync();
    }
}
