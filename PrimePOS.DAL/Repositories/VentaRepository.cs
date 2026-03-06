using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.DAL.Repositories;

public class VentaRepository
{
    private readonly AppDbContext _context;
    public VentaRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Agregar(Venta venta)
    {
        _context.Add(venta);
    }
    public void Actualizar(Venta venta)
    { 
        _context.Update(venta);
    }
    public void Anular(Venta venta)
    {
        venta.Estado = false;
        _context.Update(venta);
    }
    public Venta? ObtenerPorId(int id)
    {
        return _context.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefault(v => v.VentaId == id);
    }
    public List<Venta> ListarVentas()
    {
        return _context.Ventas.Include(v => v.Detalles).ToList();
    }
    public void GuardarCambios()
    {
        _context.SaveChanges();
    }   
}
