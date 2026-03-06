using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.DAL.Repositories;

public class UsuarioRepository 
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CrearUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public Task ActualizarUsuarioAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public Task EliminarUsuarioAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        return Task.CompletedTask;
    }

    public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
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

    public async Task<List<Rol>> ListarRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}

