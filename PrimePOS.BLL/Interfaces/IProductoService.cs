using PrimePOS.Contracts.DTOs.Producto;

namespace PrimePOS.BLL.Interfaces
{
    public interface IProductoService
    {
        Task ActualizarProductoAsync(ActualizarProductoDto dto);
        Task<ProductoDto?> BuscarProductoCodigoONombreAsync(string buscar);
        Task<List<ProductoDto>> BuscarProductoCodigoONombreListAsync(string buscar);
        Task CrearProductoAsync(CrearProductoDto dto);
        Task DesactivarProductoAsync(int productoId);
        Task EliminarProductoAsync(int productoId);
        Task<ProductoDto?> ObtenerProductoPorIdAsync(int id);
        Task<List<ProductoDto>> ObtenerTodosAsync();
    }
}