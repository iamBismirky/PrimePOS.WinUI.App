using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Catalog;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.DAL.Interfaces;

namespace PrimePOS.BLL.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _repo;

        public CatalogService(ICatalogRepository repo) { _repo = repo; }



        public async Task<List<TipoClienteDto>> ObtenerTodosTipoClientesAsync()
        {
            var tipos = await _repo.ObtenerTodosTipoClientes();
            if (tipos == null)
                throw new BusinessException("Error al obtener los métodos de pago", StatusCodes.Status404NotFound);

            return tipos.Select(m => new TipoClienteDto
            {
                TipoClienteId = m.TipoClienteId,
                Tipo = m.Nombre,

            }).ToList();
        }
        public async Task<List<TipoVentaDto>> ObtenerTodosTipoVentaAsync()
        {
            var tipos = await _repo.ObtenerTodosTipoVentas();
            if (tipos == null)
                throw new BusinessException("Error al obtener los métodos de pago", StatusCodes.Status404NotFound);

            return tipos.Select(m => new TipoVentaDto
            {
                TipoVentaId = m.TipoVentaId,
                Tipo = m.Nombre,

            }).ToList();
        }
        public async Task<List<EstadoVentaDto>> ObtenerTodosEstadoVentaAsync()
        {
            var tipos = await _repo.ObtenerTodosEstadoVentas();
            if (tipos == null)
                throw new BusinessException("Error al obtener los métodos de pago", StatusCodes.Status404NotFound);

            return tipos.Select(m => new EstadoVentaDto
            {
                EstadoVentaId = m.EstadoVentaId,
                Estado = m.Estado,

            }).ToList();
        }
        public async Task<List<MetodoPagoDto>> ObtenerTodosMetodosPagosAsync()
        {
            var metodosPago = await _repo.ObtenerTodosMetodoPagos();
            if (metodosPago == null) throw new BusinessException("Error al obtener los métodos de pago", StatusCodes.Status404NotFound);

            return metodosPago.Select(m => new MetodoPagoDto
            {
                MetodoPagoId = m.MetodoPagoId,
                Nombre = m.Nombre,
                Estado = m.Estado,

            }).ToList();

        }
    }
}
