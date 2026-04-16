using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Crear(Categoria categoria)
    {
        _context.Categorias.Add(categoria);

    }
    public void Actualizar(Categoria categoria)
    {
        _context.Categorias.Update(categoria);

    }
    public void Eliminar(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);

    }
    public async Task<Categoria?> ObtenerPorIdAsync(int categoriaId)
    {
        return await _context.Categorias.FindAsync(categoriaId);

    }
    public async Task<List<Categoria>> ListarCategoriaAsync()
    {
        return await _context.Categorias.Where(c => c.Estado == true).ToListAsync();
    }
    public async Task<Categoria?> ExisteCategoriaAsync(string nombre)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Nombre == nombre);
    }
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
