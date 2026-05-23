using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly AppDbContext _context;

        public CatalogRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TipoCliente>> ObtenerTodosTipoClientes()
        {
            return await _context.TipoClientes
                .AsNoTracking()
                .OrderBy(x => x.Tipo)
                .ToListAsync();
        }
        public async Task<List<EstadoVenta>> ObtenerTodosEstadoVentas()
        {
            return await _context.EstadoVentas
                .AsNoTracking()
                .OrderBy(x => x.Estado)
                .ToListAsync();
        }
        public async Task<List<TipoVenta>> ObtenerTodosTipoVentas()
        {
            return await _context.TipoVentas
                .AsNoTracking()
                .OrderBy(x => x.Tipo)
                .ToListAsync();
        }
        public async Task<List<MetodoPago>> ObtenerTodosMetodoPagos()
        {
            return await _context.MetodoPagos
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }
    }
}
