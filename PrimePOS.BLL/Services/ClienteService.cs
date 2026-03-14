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
    public async Task CrearClienteAsync(CrearClienteDto dto)
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
            FechaRegistro = DateTime.Now,

        };

        _clienteRepository.Crear(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        cliente.Codigo = GenerarCodigoCliente(cliente.ClienteId);

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }
    public async Task ActualizarClienteAsync(ActualizarClienteDto dto)
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
        await _clienteRepository.GuardarCambiosAsync();
    }
    public async Task EliminarClienteAsync(EliminarClienteDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado.");



        _clienteRepository.Eliminar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }
    // Listar
    public async Task<List<ClienteDto>> ListarClientes()
    {
        var clientes = await _clienteRepository.ListarClientesAsync();

        return clientes.Select(c => new ClienteDto
        {

            ClienteId = c.ClienteId,
            Codigo = c.Codigo,
            Nombre = c.Nombre,
            Documento = c.Documento,
            Direccion = c.Direccion,
            Email = c.Email,
            Telefono = c.Telefono,
            Estado = c.Estado,
            FechaRegistro = c.FechaRegistro,


        }).ToList();
    }

    // Buscar por Id
    public async Task<ClienteDto?> ObtenerPorId(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente == null)
            return null;

        return new ClienteDto
        {
            ClienteId = cliente.ClienteId,
            Codigo = cliente.Codigo,
            Nombre = cliente.Nombre,
            Documento = cliente.Documento,
            Direccion = cliente.Direccion,
            Email = cliente.Email,
            Telefono = cliente.Telefono,
            Estado = cliente.Estado,
            FechaRegistro = cliente.FechaRegistro,
        };
    }

    // Eliminar lógico
    public async Task DesactivarCliente(ClienteDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado.");

        cliente.Estado = false;

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }
    public async Task<ClienteDto?> BuscarClienteCodigoONombreAsync(string buscar)
    {
        var cliente = await _clienteRepository.BuscarPorCodigoONombreAsync(buscar);
        if (cliente == null)
            return null;

        return new ClienteDto
        {
            ClienteId = cliente.ClienteId,
            Nombre = cliente.Nombre,
            Codigo = cliente.Codigo,
            Documento = cliente.Documento,

        };

    }
    public async Task<List<ClienteDto>> BuscarClienteCodigoONombreListAsync(string buscar)
    {
        var cliente = await _clienteRepository.BuscarPorCodigoONombreListAsync(buscar);


        return cliente.Select(c => new ClienteDto
        {
            ClienteId = c.ClienteId,
            Codigo = c.Codigo,
            Nombre = c.Nombre,
            Documento = c.Documento,

        }).ToList();
    }
    private string GenerarCodigoCliente(int clienteId)
    {
        return $"CLIENT-{clienteId:D4}";
    }

}
