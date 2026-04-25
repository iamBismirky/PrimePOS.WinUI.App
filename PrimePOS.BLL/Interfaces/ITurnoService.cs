using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Interfaces
{
    public interface ITurnoService
    {
        decimal CalcularDiferencia(CierreTurnoDto cierre);
        Task CerrarTurnoAsync(CierreTurnoDto cierre);
        Task<TurnoDto> CrearTurnoDtoAsync(CrearTurnoDto dto);
        Task<CierreTurnoDto> ObtenerResumenTurno(int turnoId);
        Task<int> ObtenerSiguienteTurno();
        Task<TurnoDto?> ObtenerTurnoAbiertoAsync(int usuarioId);
        Task<TurnoDto?> ObtenerTurnoAbiertoAsync(int cajaId, int usuarioId);
        Task<List<Turno>> ObtenerTurnosPorCajaAsync(int cajaId);
    }
}