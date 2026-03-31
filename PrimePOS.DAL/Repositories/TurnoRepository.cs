using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Repositories;

public class TurnoRepository
{
    private readonly AppDbContext _context;

    public TurnoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(Turno turno)
    {
        await _context.Turnos.AddAsync(turno);
    }
    public void Actualizar(Turno turno)
    {
        _context.Turnos.Update(turno);
    }
    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExisteTurnoAbierto(int cajaId)
    {
        return await _context.Turnos.AnyAsync(t => t.CajaId == cajaId && t.EstaAbierto);
    }
    public async Task<Turno?> ObtenerPorIdAsync(int id)
    {
        return await _context.Turnos
            .FirstOrDefaultAsync(p => p.TurnoId == id);
    }
    public async Task<List<Turno>> GetTurnosPorFecha(int cajaId, DateTime fecha)
    {
        return await _context.Turnos
            .Where(t => t.CajaId == cajaId &&
                        t.FechaApertura.Date == fecha)
            .ToListAsync();
    }
    public async Task<Turno?> ObtenerUltimoTurnoDelDiaAsync(int cajaId, DateTime fecha)
    {
        return await _context.Turnos
            .Where(t => t.CajaId == cajaId &&
                        t.FechaApertura.Date == fecha.Date)
            .OrderByDescending(t => t.TurnoId)
            .FirstOrDefaultAsync();
    }
    public async Task<List<Turno>> ObtenerPorCajaAsync(int cajaId)
    {
        return await _context.Turnos
            .Where(t => t.CajaId == cajaId)
            .OrderByDescending(t => t.FechaApertura)
            .ToListAsync();
    }
    public async Task<Turno?> ObtenerUltimoTurnoDelDia(DateTime fecha)
    {
        return await _context.Turnos
            .Where(x => x.FechaApertura.Date == fecha)
            .OrderByDescending(x => x.NumeroTurno)
            .FirstOrDefaultAsync();
    }
    public async Task<Turno?> ObtenerTurnoAbiertoPorUsuarioAsync(int usuarioId)
    {
        return await _context.Turnos.Where(t => t.UsuarioId == usuarioId && t.FechaCierre == null)
            .OrderByDescending(t => t.FechaApertura)
            .FirstOrDefaultAsync();

    }
    public async Task<Turno?> ObtenerTurnoAbiertoPorCajaAsync(int cajaId)
    {
        return await _context.Turnos
            .Where(t => t.CajaId == cajaId && t.FechaCierre == null)
            .OrderByDescending(t => t.FechaApertura)
            .FirstOrDefaultAsync();
    }
}
