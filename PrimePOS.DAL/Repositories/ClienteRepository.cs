using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class ClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;

    }
    public void Crear(Cliente cliente)
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
    public async Task<List<Cliente>> ListarClientesAsync()
    {
        return await _context.Clientes.ToListAsync();
    }
    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task GuardarCambios()
    {
        await _context.SaveChangesAsync();
    }

}
