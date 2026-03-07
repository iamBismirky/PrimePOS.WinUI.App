using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.DAL.Repositories;

public class RolRepository
{
    private readonly AppDbContext _context;
    public RolRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task CrearRolAsync(Rol rol)
    {
       await _context.Roles.AddAsync(rol);

    }
    public Task ActualizarRolAsync(Rol rol)
    {
        _context.Roles.Update(rol);
        return Task.CompletedTask;
    }
    public Task EliminarRolAsync(Rol rol)
    {
        _context.Roles.Remove(rol);
        return Task.CompletedTask;
    }
    public async Task<Rol?> ObtenerPorIdAsync(int rolId)
    {
        return await _context.Roles.FindAsync(rolId);

    }
    public async Task<List<Rol>> ListarRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }
    public async Task<bool> ExisteRol(string nombre)
    {
        return await _context.Roles
            .AnyAsync(r => r.Nombre == nombre);
    }
    public Task GuardarCambiosAsync()
    {
        return _context.SaveChangesAsync();
    }

}
