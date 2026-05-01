using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class CajaRepository : ICajaRepository
    {
        private readonly AppDbContext _context;

        public CajaRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Crear(Caja caja)
        {
            _context.Cajas.Add(caja);
        }
        public void Actualizar(Caja caja)
        {
            _context.Cajas.Update(caja);
        }
        public void Eliminar(Caja caja)
        {
            _context.Cajas.Remove(caja);
        }
        public async Task<List<Caja>> ObtenerTodosAsync()
        {
            return await _context.Cajas.ToListAsync();
        }

        public async Task<Caja?> ObtenerCajaPorIdAsync(int id)
        {
            return await _context.Cajas.FindAsync(id);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Caja?> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.Cajas
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == c.Nombre.ToLower());
        }
        public async Task<Caja?> ExisteCajaIdAsync(int cajaId)
        {
            return await _context.Cajas
                .FirstOrDefaultAsync(c => c.CajaId == cajaId);
        }
    }
}
