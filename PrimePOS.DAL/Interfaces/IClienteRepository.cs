using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IClienteRepository
    {
        void Actualizar(Cliente cliente);
        Task<List<Cliente>> BuscarAsync(string texto);
        Task<Cliente?> BuscarClientePorCodigoAsync(string codigo);
        Task<Cliente?> BuscarClientePorNombreAsync(string nombre);
        Task<Cliente?> BuscarPorCodigoONombreAsync(string buscar);
        Task<List<Cliente>> BuscarPorCodigoONombreListAsync(string buscar);
        void Crear(Cliente cliente);
        void Eliminar(Cliente cliente);
        Task GuardarCambiosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<List<Cliente>> ObtenerTodosAsync();
    }
}