using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Clientes;

namespace PrimePOS.DAL.Repositories;

public class ClienteRepository : IClienteRepository
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
    public async Task<List<Cliente>> ObtenerTodosAsync()
    {
        return await _context.Clientes
            .Include(c => c.TipoCliente)
            .ToListAsync();
    }
    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _context.Clientes
        .Include(x => x.TipoCliente)
        .FirstOrDefaultAsync(x => x.ClienteId == id);
    }
    public async Task<Cliente?> BuscarPorCodigoONombreAsync(string buscar)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Codigo == buscar || c.Nombre!.Contains(buscar));
    }
    public async Task<Cliente?> BuscarClientePorCodigoAsync(string codigo)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Codigo == codigo);
    }
    public async Task<Cliente?> BuscarClientePorNombreAsync(string nombre)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Nombre.Contains(nombre));
    }
    public async Task<List<Cliente>> BuscarPorCodigoONombreListAsync(string buscar)
    {
        return await _context.Clientes
            .Where(c => c.Codigo.Contains(buscar) || c.Nombre!.Contains(buscar)).Take(10)
        .ToListAsync();
    }
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<Cliente>> BuscarAsync(string texto)
    {
        texto = texto.ToLower();

        return await _context.Clientes
            .Include(c => c.TipoCliente)
            .Where(p =>
                p.Estado == true &&
                (
                    p.Nombre.ToLower().Contains(texto) ||
                    p.Codigo.ToLower().Contains(texto) ||
                    p.Documento.ToLower().Contains(texto)
                )
            )
            .OrderBy(p => p.Nombre)
            .Take(20)
            .ToListAsync();
    }
    public async Task<int> CountAsync()
    {
        return await _context.Clientes.CountAsync();
    }
    public async Task<Cliente?> CargarConsumidorFinalAsync()
    {
        return await _context.Clientes
            .Include(c => c.TipoCliente)
            .Where(c => c.ClienteId == 1)
            .FirstOrDefaultAsync();
    }
}
