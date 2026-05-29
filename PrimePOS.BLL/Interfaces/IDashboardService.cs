using PrimePOS.Contracts.DTOs.Dashboard;

namespace PrimePOS.BLL.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardInventoryDto> ObtenerResumenInventarioAsync();
        Task<DashboardResumeDto> ObtenerVentasDelDiaAsync(int turnoId);
    }
}