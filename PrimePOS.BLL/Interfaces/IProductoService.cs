using PrimePOS.Contracts.DTOs.Producto;

namespace PrimePOS.BLL.Interfaces
{
    public interface IProductoService
    {
        Task CrearProductoAsync(CrearProductoDto dto);
        Task ActualizarProductoAsync(ActualizarProductoDto dto);
        Task DesactivarProductoAsync(int productoId);
        Task EliminarProductoAsync(int productoId);
        Task<ProductoDto?> ObtenerPorIdAsync(int id);
        Task<List<ProductoDto>> ObtenerTodosAsync();
        Task<List<ProductoDto>> BuscarProductosAsync(string texto);


    }
}