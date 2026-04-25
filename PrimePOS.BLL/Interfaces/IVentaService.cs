using PrimePOS.Contracts.DTOs.Venta;

namespace PrimePOS.BLL.Interfaces
{
    public interface IVentaService
    {
        Task<int> CrearVentaAsync(CrearVentaDto dto);
        Task<List<VentaDto>> ObtenerVentasDelDiaAsync();
        Task<List<VentaDto>> ObtenerVentasPorTurnoAsync(int turnoId);
    }
}