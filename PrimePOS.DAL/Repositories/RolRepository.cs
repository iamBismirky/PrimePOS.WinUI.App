using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class RolRepository
{
    private readonly AppDbContext _context;
    public RolRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Crear(Rol rol)
    {
        _context.Roles.Add(rol);

    }
    public void Actualizar(Rol rol)
    {
        _context.Roles.Update(rol);
        
    }
    public void Eliminar(Rol rol)
    {
        _context.Roles.Remove(rol);
        
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
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

}
