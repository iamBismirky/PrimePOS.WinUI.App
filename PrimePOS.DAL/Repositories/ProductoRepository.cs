using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class ProductoRepository : IProductoRepository
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
    public async Task<List<Producto>> ObtenerTodosAsync()
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
    public async Task<Producto?> BuscarPorNombreAsync(string nombre)
    {
        return await _context.Productos.FirstOrDefaultAsync(p => p.Nombre.Contains(nombre));
    }
    public async Task<Producto?> BuscarPorCodigoONombreAsync(string buscar)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Codigo == buscar || p.Nombre!.Contains(buscar));
    }
    public async Task<List<Producto>> BuscarPorCodigoONombreListAsync(string buscar)
    {
        return await _context.Productos
            .Where(p => p.Codigo.Contains(buscar) || p.Nombre!.Contains(buscar)).Take(10)
        .ToListAsync();
    }
    public async Task<bool> ExisteCodigoONombreAsync(string codigoBarra, string nombre)
    {
        return await _context.Productos
            .AnyAsync(p => p.CodigoBarra == codigoBarra || p.Nombre == nombre);
    }
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<Producto>> BuscarAsync(string texto)
    {
        texto = texto.ToLower();

        return await _context.Productos
            .Where(p =>
                p.Estado == true &&
                (
                    p.Nombre.ToLower().Contains(texto) ||
                    p.Codigo.ToLower().Contains(texto) ||
                    p.CodigoBarra.ToLower().Contains(texto)
                )
            )
            .OrderBy(p => p.Nombre)
            .Take(20)
            .ToListAsync();
    }
}
