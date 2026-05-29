using Microsoft.AspNetCore.Mvc;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Dashboard;

namespace PrimePOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardRepo;

        public DashboardController(IDashboardService dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }
        [HttpGet("turno/{turnoId}/resumen")]
        public async Task<IActionResult> GetDashboard(int turnoId)
        {
            var result = await _dashboardRepo.ObtenerVentasDelDiaAsync(turnoId);



            return Ok(new ApiResponse<DashboardResumeDto>
            {
                Success = true,
                Data = result,
                Message = "Datos del dashboard obtenidos correctamente",
            });
        }
        [HttpGet("inventario")]
        public async Task<IActionResult> ObtenerResumenInventario()
        {
            var result = await _dashboardRepo.ObtenerResumenInventarioAsync();


            return Ok(new ApiResponse<DashboardInventoryDto>
            {
                Success = true,
                Data = result,
                Message = "Datos del dashboard obtenidos correctamente",
            });
        }
    }
}