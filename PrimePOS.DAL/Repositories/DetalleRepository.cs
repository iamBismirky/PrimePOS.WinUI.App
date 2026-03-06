using PrimePOS.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class DetalleRepository
{
    private readonly AppDbContext _context;
    public DetalleRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Agregar(DetalleVenta detalle)
    {
        _context.Add(detalle);
        _context.SaveChanges();
    }
}
