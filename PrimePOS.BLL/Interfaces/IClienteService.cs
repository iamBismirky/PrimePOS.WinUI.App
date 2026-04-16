using PrimePOS.Contracts.DTOs.Cliente;

namespace PrimePOS.BLL.Interfaces
{
    public interface IClienteService
    {
        Task ActualizarClienteAsync(ActualizarClienteDto dto);
        Task<ClienteDto?> BuscarClienteCodigoONombreAsync(string buscar);
        Task<List<ClienteDto>> BuscarClienteCodigoONombreListAsync(string buscar);
        Task CrearClienteAsync(CrearClienteDto dto);
        Task DesactivarClienteAsync(int clienteId);
        Task EliminarClienteAsync(int clienteId);
        Task<List<ClienteDto>> ObtenerTodosAsync();
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
    }
}