using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Clientes;
using PrimePOS.ENTITIES.Models.Ventas;

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
                .OrderBy(x => x.Nombre)
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
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }
        public async Task<List<MetodoPago>> ObtenerTodosMetodoPagos()
        {
            return await _context.MetodoPagos
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }
        public async Task<bool> ExisteTipoPrecioAsync(int id)
        {
            return await _context.TipoPrecios
                .AnyAsync(x => x.TipoPrecioId == id);
        }

        public async Task<bool> ExisteTipoVentaAsync(int id)
        {
            return await _context.TipoVentas
                .AnyAsync(x => x.TipoVentaId == id);
        }


        #region Listados

        public async Task<List<TipoCliente>> ObtenerTodosTipoClientesAsync()
        {
            return await _context.TipoClientes
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<TipoVenta>> ObtenerTodosTipoVentasAsync()
        {
            return await _context.TipoVentas
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<TipoPrecio>> ObtenerTodosTipoPreciosAsync()
        {
            return await _context.TipoPrecios
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<MetodoPago>> ObtenerTodosMetodoPagosAsync()
        {
            return await _context.MetodoPagos
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<EstadoVenta>> ObtenerTodosEstadoVentasAsync()
        {
            return await _context.EstadoVentas
                .AsNoTracking()
                .OrderBy(x => x.Estado)
                .ToListAsync();
        }

        #endregion

        #region Obtener por Id

        public async Task<TipoVenta?> ObtenerTipoVentaAsync(int id)
        {
            return await _context.TipoVentas
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TipoVentaId == id);
        }

        public async Task<TipoPrecio?> ObtenerTipoPrecioAsync(int id)
        {
            return await _context.TipoPrecios
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TipoPrecioId == id);
        }

        public async Task<MetodoPago?> ObtenerMetodoPagoAsync(int id)
        {
            return await _context.MetodoPagos
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.MetodoPagoId == id);
        }

        public async Task<EstadoVenta?> ObtenerEstadoVentaAsync(int id)
        {
            return await _context.EstadoVentas
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.EstadoVentaId == id);
        }

        #endregion
    }
}
