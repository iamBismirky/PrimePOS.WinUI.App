using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IProductoRepository
    {
        void Actualizar(Producto producto);
        Task<List<Producto>> BuscarAsync(string texto);
        Task<Producto?> BuscarPorCodigoAsync(string codigo);
        Task<Producto?> BuscarPorCodigoONombreAsync(string buscar);
        Task<List<Producto>> BuscarPorCodigoONombreListAsync(string buscar);
        Task<Producto?> BuscarPorNombreAsync(string nombre);
        void Crear(Producto producto);
        void Eliminar(Producto producto);
        Task<bool> ExisteCodigoONombreAsync(string codigoBarra, string nombre);
        Task GuardarCambiosAsync();
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<List<Producto>> ObtenerTodosAsync();
    }
}