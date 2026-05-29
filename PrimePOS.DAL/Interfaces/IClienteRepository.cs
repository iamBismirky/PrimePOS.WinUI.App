using PrimePOS.ENTITIES.Models.Clientes;

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
        Task<Cliente?> CargarConsumidorFinalAsync();
        Task<int> CountAsync();
        void Crear(Cliente cliente);
        void Eliminar(Cliente cliente);
        Task GuardarCambiosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<List<Cliente>> ObtenerTodosAsync();
    }
}