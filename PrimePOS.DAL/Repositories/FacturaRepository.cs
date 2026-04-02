using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class FacturaRepository
    {
        private readonly AppDbContext _context;

        public FacturaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Factura factura)
        {
            await _context.Facturas.AddAsync(factura);
        }

        public async Task<Factura?> ObtenerPorIdAsync(int id)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefaultAsync(f => f.FacturaId == id);
        }

        public async Task<List<Factura>> ObtenerTodosAsync()
        {
            return await _context.Facturas.ToListAsync();
        }
        public async Task<int> ContarFacturasAsync()
        {
            return await _context.Facturas.CountAsync();
        }
        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
