using PrimePOS.Contracts.DTOs.Venta;

namespace PrimePOS.BLL.Interfaces
{
    public interface IVentaService
    {
        Task<int> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto);
        Task<List<VentaDto>> ObtenerVentasDelDiaAsync();
        Task<decimal> ObtenerVentasPorTurnoAsync(int turnoId);
    }
}