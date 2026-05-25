using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Clientes;

namespace PrimePOS.BLL.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ICatalogRepository _catalogRepo;

    public ClienteService(IClienteRepository clienteRepository, ICatalogRepository catalogRepo)
    {
        _clienteRepository = clienteRepository;
        _catalogRepo = catalogRepo;
    }

    public async Task CrearClienteAsync(CrearClienteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del cliente es obligatorio.", StatusCodes.Status400BadRequest);

        if (string.IsNullOrWhiteSpace(dto.Documento))
            throw new BusinessException("El documento del cliente es obligatorio.", StatusCodes.Status400BadRequest);

        if (dto.TipoClienteId <= 0)
            throw new BusinessException("El tipo de cliente es obligatorio.", StatusCodes.Status400BadRequest);

        var tipoCliente = await _catalogRepo.ObtenerTipoPrecioAsync(dto.TipoClienteId);

        if (tipoCliente == null)
            throw new BusinessException("Tipo de cliente inválido", StatusCodes.Status400BadRequest);

        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Documento = dto.Documento,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            Estado = dto.Estado,
            FechaRegistro = DateTime.Now,
            TipoClienteId = dto.TipoClienteId,
            TipoPrecioId = tipoCliente.TipoPrecioId,

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
            throw new BusinessException("El nombre del cliente es obligatorio.", StatusCodes.Status400BadRequest);

        if (string.IsNullOrWhiteSpace(dto.Documento))
            throw new BusinessException("El documento del cliente es obligatorio.", StatusCodes.Status400BadRequest);

        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", StatusCodes.Status404NotFound);

        var tipoCliente = await _catalogRepo.ObtenerTipoPrecioAsync(dto.TipoClienteId);

        if (tipoCliente == null)
            throw new BusinessException("Tipo de cliente inválido", StatusCodes.Status400BadRequest);

        cliente.Nombre = dto.Nombre;
        cliente.Documento = dto.Documento;
        cliente.Direccion = dto.Direccion;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Estado = dto.Estado;
        cliente.TipoClienteId = dto.TipoClienteId;
        cliente.TipoPrecioId = tipoCliente.TipoPrecioId;

        _clienteRepository.Actualizar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }

    public async Task EliminarClienteAsync(int clienteId)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", StatusCodes.Status404NotFound);

        _clienteRepository.Eliminar(cliente);
        await _clienteRepository.GuardarCambiosAsync();
    }

    public async Task DesactivarClienteAsync(int clienteId)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", StatusCodes.Status404NotFound);

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
            TipoClienteId = c.TipoClienteId,
            TipoPrecioId = c.TipoPrecioId,
            Tipo = c.TipoCliente?.Nombre ?? "",
            FechaRegistro = c.FechaRegistro,
        }).ToList();
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente == null)
            throw new BusinessException("Cliente no encontrado.", StatusCodes.Status404NotFound);

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
            TipoClienteId = cliente.TipoClienteId,
            TipoPrecioId = cliente.TipoPrecioId,
            Tipo = cliente.TipoCliente?.Nombre ?? "",
        };
    }




    private string GenerarCodigoCliente(int clienteId)
    {
        return $"CLIENT-{clienteId:D4}";
    }
    public async Task<List<ClienteDto>> BuscarClientesAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return new List<ClienteDto>();

        var clientes = await _clienteRepository.BuscarAsync(texto);
        if (clientes == null)
            throw new BusinessException("Cliente no encontrado.", StatusCodes.Status404NotFound);
        return clientes.Select(c => new ClienteDto
        {
            ClienteId = c.ClienteId,
            Codigo = c.Codigo,
            Nombre = c.Nombre,
            Documento = c.Documento,
            TipoClienteId = c.TipoClienteId,
            Tipo = c.TipoCliente?.Nombre ?? "",
            TipoPrecioId = c.TipoPrecioId,

        }).ToList();
    }
}