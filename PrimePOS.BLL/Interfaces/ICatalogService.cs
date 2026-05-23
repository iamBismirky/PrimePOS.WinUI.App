using PrimePOS.Contracts.DTOs.Catalog;
using PrimePOS.Contracts.DTOs.MetodoPago;

namespace PrimePOS.BLL.Interfaces
{
    public interface ICatalogService
    {
        Task<List<EstadoVentaDto>> ObtenerTodosEstadoVentaAsync();
        Task<List<MetodoPagoDto>> ObtenerTodosMetodosPagosAsync();
        Task<List<TipoClienteDto>> ObtenerTodosTipoClientesAsync();
        Task<List<TipoVentaDto>> ObtenerTodosTipoVentaAsync();
    }
}