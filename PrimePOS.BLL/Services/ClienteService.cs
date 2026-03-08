using PrimePOS.BLL.DTOs.Cliente;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;


public class ClienteService
{
    private readonly ClienteRepository _clienteRepository;
    public ClienteService(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }
    public async Task CrearClienteAsync(ClienteDto dto)
    {


        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("El nombre del cliente es obligatorio.");
        if (!dto.Estado)
            throw new Exception("El estado del cliente es obligatorio.");

        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Documento = dto.Documento,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            Estado = dto.Estado,

        };

        _clienteRepository.Crear(cliente);
        await _clienteRepository.GuardarCambios();
    }
    public async Task ActualizarCliente(ClienteDto dto)
    {
        if (dto == null)
            throw new Exception("Datos inválidos.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("El nombre del cliente es obligatorio.");


        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId)
            ?? throw new Exception("Cliente no encontrado.");

        if (cliente == null)
            throw new Exception("Cliente no encontrado");

        cliente.Nombre = dto.Nombre;
        cliente.Documento = dto.Documento;
        cliente.Direccion = dto.Direccion;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Estado = dto.Estado;


        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambios();
    }
    public async Task EliminarCliente(ClienteDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado.");



        _clienteRepository.Eliminar(cliente);
        await _clienteRepository.GuardarCambios();
    }
    // Listar
    public async Task<List<Cliente>> ListarClientes()
    {
        return await _clienteRepository.ListarClientesAsync();
    }

    // Buscar por Id
    public async Task<Cliente?> ObtenerPorId(int id)
    {
        return await _clienteRepository.ObtenerPorIdAsync(id);
    }

    // Eliminar lógico
    public async Task DesactivarCliente(ClienteDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado.");

        cliente.Estado = false;

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambios();
    }


}
