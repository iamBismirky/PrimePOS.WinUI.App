using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class ProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Crear(Producto producto)
    {
        _context.Productos.Add(producto);
    }

    public void Actualizar(Producto producto)
    {
        _context.Productos.Update(producto);
    }

    public void Eliminar(Producto producto)
    {
        _context.Productos.Remove(producto);
    }
    public async Task<List<Producto>> ListarAsync()
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.ProductoId == id);
    }

    public async Task<Producto?> BuscarPorCodigoAsync(string codigo)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Codigo == codigo);
    }

    public async Task<Producto?> BuscarPorCodigoONombreAsync(string buscar)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Codigo == buscar || p.Nombre!.Contains(buscar));
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? excluirId = null)
    {
        return await _context.Productos
            .AnyAsync(p => p.Codigo == codigo &&
                      (excluirId == null || p.ProductoId != excluirId));
    }
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
