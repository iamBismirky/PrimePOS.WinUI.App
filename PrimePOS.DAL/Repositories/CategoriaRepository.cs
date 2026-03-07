using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.DAL.Repositories;

public class CategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CrearCategoriaAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);

    }
    public Task ActualizarCategoriaAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        return Task.CompletedTask;
    }
    public Task EliminarCategoriaAsync(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        return Task.CompletedTask;
    }
    public async Task<Categoria?> ObtenerPorIdAsync(int categoriaId)
    {
        return await _context.Categorias.FindAsync(categoriaId);

    }
    public async Task<List<Categoria>> ListarCategoriaAsync()
    {
        return await _context.Categorias.ToListAsync();
    }
    public async Task<bool> ExisteCategoriaAsync(string nombre)
    {
        return await _context.Categorias
            .AnyAsync(c => c.Nombre == nombre);
    }
    public Task GuardarCambiosAsync()
    {
        return _context.SaveChangesAsync();
    }
}
