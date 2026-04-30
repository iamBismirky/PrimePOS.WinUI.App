using PrimePOS.Contracts.DTOs.Venta;

namespace PrimePOS.BLL.Interfaces
{
    public interface IVentaService
    {
        Task<VentaResponseDto> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto);
        Task<List<VentaDto>> ObtenerVentasDelDiaAsync();
        Task<List<VentaDto>> ObtenerVentasPorTurnoAsync(int turnoId);
    }
}