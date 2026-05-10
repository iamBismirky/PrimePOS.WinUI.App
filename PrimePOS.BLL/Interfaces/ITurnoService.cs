using PrimePOS.Contracts.DTOs.Turno;

namespace PrimePOS.BLL.Interfaces
{
    public interface ITurnoService
    {
        decimal CalcularDiferencia(CierreTurnoDto cierre);
        Task CerrarTurnoAsync(TurnoDto cierre);
        Task<TurnoDto> CrearTurnoDtoAsync(CrearTurnoDto dto);
        Task<CierreTurnoDto> ObtenerResumenTurno(int turnoId);
        Task<int> ObtenerSiguienteTurno();
        Task<TurnoDto?> ObtenerTurnoAbiertoAsync(int usuarioId);
    }
}