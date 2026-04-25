using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class DetalleRepository : IDetalleRepository
{
    private readonly AppDbContext _context;
    public DetalleRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Agregar(VentaDetalle detalle)
    {
        _context.Add(detalle);
        _context.SaveChanges();
    }
}
