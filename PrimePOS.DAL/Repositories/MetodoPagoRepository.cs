using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class MetodoPagoRepository
    {
        private readonly AppDbContext _context;
        public MetodoPagoRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<MetodoPago?> ObtenerPorIdAsync(int id)
        {
            return await _context.MetodoPagos.FirstOrDefaultAsync(m => m.MetodoPagoId == id);

        }
        public async Task<List<MetodoPago>> ListarMetodosPagosAsync()
        {
            return await _context.MetodoPagos.ToListAsync();
        }
    }

}
