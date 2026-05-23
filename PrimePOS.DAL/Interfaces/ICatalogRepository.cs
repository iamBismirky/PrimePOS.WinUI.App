using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface ICatalogRepository
    {
        Task<List<EstadoVenta>> ObtenerTodosEstadoVentas();
        Task<List<MetodoPago>> ObtenerTodosMetodoPagos();
        Task<List<TipoCliente>> ObtenerTodosTipoClientes();
        Task<List<TipoVenta>> ObtenerTodosTipoVentas();
    }
}