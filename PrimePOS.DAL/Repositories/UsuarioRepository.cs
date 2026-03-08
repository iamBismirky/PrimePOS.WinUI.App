using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class UsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Crear(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
    }

    public void Actualizar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);

    }

    public void Eliminar(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);

    }

    public async Task<Usuario?> ObtenerPorId(int id)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);
    }

    public async Task<List<Usuario>> ListarUsuariosAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .ToListAsync();
    }

    public async Task<bool> ExisteUsernameAsync(string username, int? excluirId = null)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Username == username &&
                          (excluirId == null || u.UsuarioId != excluirId));
    }

    public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

  

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}

