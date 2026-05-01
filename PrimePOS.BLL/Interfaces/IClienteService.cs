using PrimePOS.Contracts.DTOs.Cliente;

namespace PrimePOS.BLL.Interfaces
{
    public interface IClienteService
    {
        Task CrearClienteAsync(CrearClienteDto dto);
        Task ActualizarClienteAsync(ActualizarClienteDto dto);
        Task DesactivarClienteAsync(int clienteId);
        Task EliminarClienteAsync(int clienteId);
        Task<List<ClienteDto>> ObtenerTodosAsync();
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
        Task<List<ClienteDto>> BuscarClientesAsync(string texto);
    }
}