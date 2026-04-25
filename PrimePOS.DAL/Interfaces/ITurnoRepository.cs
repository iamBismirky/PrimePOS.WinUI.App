using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface ITurnoRepository
    {
        void Actualizar(Turno turno);
        Task AgregarAsync(Turno turno);
        Task<bool> ExisteTurnoAbierto(int cajaId);
        Task<List<Turno>> GetTurnosPorFecha(int cajaId, DateTime fecha);
        Task GuardarCambiosAsync();
        Task<List<Turno>> ObtenerPorCajaAsync(int cajaId);
        Task<Turno?> ObtenerPorIdAsync(int id);
        Task<Turno?> ObtenerTurnoAbiertoAsync(int cajaId, int usuarioId);
        Task<Turno?> ObtenerTurnoAbiertoPorUsuarioAsync(int usuarioId);
        Task<Turno?> ObtenerUltimoTurnoDelDia(DateTime fecha);
        Task<Turno?> ObtenerUltimoTurnoDelDiaAsync(int cajaId, DateTime fecha);
    }
}