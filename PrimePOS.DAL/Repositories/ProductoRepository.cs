using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.DAL.Repositories;

public class ProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Producto> Listar()
    {
        return _context.Productos
            .Include(p => p.Categoria)
            .ToList();
    }

    public void Agregar(Producto producto)
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

    public Producto? ObtenerPorId(int id)
    {
        return _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefault(p => p.ProductoId == id);
    }

    public Producto? BuscarPorCodigo(string codigo)
    {
        return _context.Productos
            .FirstOrDefault(p => p.Codigo == codigo);
    }

    public Producto? BuscarPorCodigoONombre(string buscar)
    {
        return _context.Productos
            .FirstOrDefault(p => p.Codigo == buscar || p.Nombre!.Contains(buscar));
    }

    public bool ExisteCodigo(string codigo, int? excluirId = null)
    {
        return _context.Productos
            .Any(p => p.Codigo == codigo &&
                      (excluirId == null || p.ProductoId != excluirId));
    }
    public void GuardarCambios()
    {
        _context.SaveChanges();
    }   
}
