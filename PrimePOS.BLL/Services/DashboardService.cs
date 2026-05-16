using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Dashboard;

namespace PrimePOS.BLL.Services
{
    public class DashboardService
    {
        private readonly IVentaService _ventaService;
        public DashboardService(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }
        public async Task<DashboardResumeDto> ObtenerVentasDelDiaAsync(int turnoId)
        {
            return new DashboardResumeDto
            {
                VentasDelDia = await _ventaService.ObtenerVentasPorTurnoAsync(turnoId),
                //VentasCount = await _ventaService.ObtenerVentasCountAsync(),
                //ClienteCount = await _ventaService.ObtenerClientesCountAsync(),
                //ProductoCount = await _ventaService.ObtenerProductosCountAsync()
            };
        }
    }
}
