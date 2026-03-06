using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.Services;


public class ClienteService  
{
    private readonly ClienteRepository _clienteRepository;
    public ClienteService (ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }
    public void AgregarCliente(Cliente cliente)
    {
        if (cliente == null) throw new ArgumentNullException(nameof(cliente));

        if (string.IsNullOrWhiteSpace(cliente.Nombre))
            throw new Exception("El nombre del cliente es obligatorio.");
        if(cliente.Estado == null)
            throw new Exception("El estado del cliente es obligatorio.");



        _clienteRepository.Agregar(cliente);
        _clienteRepository.GuardarCambios();
    }
    // Listar
    public List<Cliente> ListarClientes()
    {
        return _clienteRepository.Listar();
    }

    // Buscar por Id
    public Cliente? ObtenerPorId(int id)
    {
        return _clienteRepository.ObtenerPorId(id);
    }

    // Eliminar lógico
    public void DesactivarCliente(int id)
    {
        var cliente = _clienteRepository.ObtenerPorId(id);

        if (cliente == null)
            throw new Exception("Cliente no encontrado.");

        cliente.Estado = false;

        _clienteRepository.Actualizar(cliente);
        _clienteRepository.GuardarCambios();
    }
    public void ActualizarCliente(Cliente cliente)
    {
        if (cliente == null)
            throw new Exception("Datos inválidos.");

        if (string.IsNullOrWhiteSpace(cliente.Nombre))
            throw new Exception("El nombre del cliente es obligatorio.");
        if (cliente.Estado == null)
            throw new Exception("El estado del cliente es obligatorio.");


        var clienteBD = _clienteRepository.ObtenerPorId(cliente.ClienteId)
            ?? throw new Exception("Cliente no encontrado.");

        

        clienteBD.Nombre = cliente.Nombre;
        clienteBD.Documento = cliente.Documento;
        clienteBD.Direccion = cliente.Direccion;
        clienteBD.Email = cliente.Email;
        clienteBD.Telefono = cliente.Telefono;


        _clienteRepository.Actualizar(clienteBD);
        _clienteRepository.GuardarCambios();
    }
    public void EliminarCliente(int id)
    {
        var cliente = _clienteRepository.ObtenerPorId(id);

        if (cliente == null)
            throw new Exception("Cliente no encontrado.");

       

        _clienteRepository.Eliminar(cliente);
        _clienteRepository.GuardarCambios();
    }
}
