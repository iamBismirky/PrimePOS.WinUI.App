using PrimePOS.ENTITIES.Models.Clientes;
using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Interfaces
{
    public interface ICatalogRepository
    {
        Task<bool> ExisteTipoPrecioAsync(int id);
        Task<bool> ExisteTipoVentaAsync(int id);
        Task<EstadoVenta?> ObtenerEstadoVentaAsync(int id);
        Task<MetodoPago?> ObtenerMetodoPagoAsync(int id);
        Task<TipoPrecio?> ObtenerTipoPrecioAsync(int id);
        Task<TipoVenta?> ObtenerTipoVentaAsync(int id);
        Task<List<EstadoVenta>> ObtenerTodosEstadoVentas();
        Task<List<EstadoVenta>> ObtenerTodosEstadoVentasAsync();
        Task<List<MetodoPago>> ObtenerTodosMetodoPagos();
        Task<List<MetodoPago>> ObtenerTodosMetodoPagosAsync();
        Task<List<TipoCliente>> ObtenerTodosTipoClientes();
        Task<List<TipoCliente>> ObtenerTodosTipoClientesAsync();
        Task<List<TipoPrecio>> ObtenerTodosTipoPreciosAsync();
        Task<List<TipoVenta>> ObtenerTodosTipoVentas();
        Task<List<TipoVenta>> ObtenerTodosTipoVentasAsync();
    }
}