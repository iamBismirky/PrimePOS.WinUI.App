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

    public void Agregar(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
    }

    public List<Categoria> Listar()
    {
        return _context.Categorias.ToList();
    }

    public Categoria? ObtenerPorId(int id)
    {
        return _context.Categorias.Find(id);
    }

    public void Actualizar(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        _context.SaveChanges();
    }

    public void Eliminar(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
    }

    public bool ExisteNombre(string nombre, int? excluirId = null)
    {
        return _context.Categorias
            .Any(c => c.Nombre == nombre &&
                     (excluirId == null || c.CategoriaId != excluirId));
    }
}
