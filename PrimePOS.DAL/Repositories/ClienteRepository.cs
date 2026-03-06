using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.DAL.Repositories;

public class ClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }
    public List<Cliente> Listar()
    {
        return _context.Clientes.ToList();
    }
    public Cliente? ObtenerPorId(int id)
    {
        return _context.Clientes.Find(id);
    }
    public void Agregar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
    }
    public void Actualizar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
    }
    public void Eliminar(Cliente cliente)
    {
        
        _context.Clientes.Remove(cliente);
    }
    public void GuardarCambios()
    {
        _context.SaveChanges();
    }

}
