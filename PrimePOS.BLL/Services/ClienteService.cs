using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task CrearClienteAsync(CrearClienteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del cliente es obligatorio.", 400);

        if (string.IsNullOrWhiteSpace(dto.Documento))
            throw new BusinessException("El documento del cliente es obligatorio.", 400);

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
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del cliente es obligatorio.", 400);

        if (string.IsNullOrWhiteSpace(dto.Documento))
            throw new BusinessException("El documento del cliente es obligatorio.", 400);

        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", 404);

        cliente.Nombre = dto.Nombre;
        cliente.Documento = dto.Documento;
        cliente.Direccion = dto.Direccion;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Estado = dto.Estado;

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }

    public async Task EliminarClienteAsync(int clienteId)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", 404);

        _clienteRepository.Eliminar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }

    public async Task DesactivarClienteAsync(int clienteId)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", 404);

        cliente.Estado = false;

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }

    public async Task<List<ClienteDto>> ObtenerTodosAsync()
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync();

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

    public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", 404);

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

    public async Task<ClienteDto?> BuscarClienteCodigoONombreAsync(string buscar)
    {
        buscar = buscar.Trim();

        var cliente = await _clienteRepository.BuscarPorCodigoONombreAsync(buscar)
                      ?? await _clienteRepository.BuscarClientePorNombreAsync(buscar);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", 404);

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
        var clientes = await _clienteRepository.BuscarPorCodigoONombreListAsync(buscar);

        return clientes.Select(c => new ClienteDto
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