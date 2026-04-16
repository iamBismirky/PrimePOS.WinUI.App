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
        public async Task<List<Caja>> ListarCajasAsync()
        {
            return await _context.Cajas
                .Where(c => c.Estado == true).ToListAsync();
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
                .FirstOrDefaultAsync(c => c.Nombre == c.Nombre);
        }
        public async Task<bool> ExisteCajaIdAsync(int cajaId)
        {
            return await _context.Turnos
                .AnyAsync(c => c.CajaId == cajaId);
        }
    }
}
