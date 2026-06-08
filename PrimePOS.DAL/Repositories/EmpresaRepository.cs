using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Seguridad;

namespace PrimePOS.DAL.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly AppDbContext _context;
        public EmpresaRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Crear(Empresa empresa)
        {
            _context.Empresas.Add(empresa);
        }
        public void Actualizar(Empresa empresa)
        {
            _context.Empresas.Update(empresa);
        }
        public void Eliminar(Empresa empresa)
        {

            _context.Empresas.Remove(empresa);
        }
        public async Task<Empresa?> ObtenerPorIdAsync(int id)
        {
            return await _context.Empresas.FirstOrDefaultAsync(e => e.EmpresaId == id);
        }
        public async Task<List<Empresa>> ObtenerTodosAsync()
        {
            return await _context.Empresas.ToListAsync();
        }
        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
