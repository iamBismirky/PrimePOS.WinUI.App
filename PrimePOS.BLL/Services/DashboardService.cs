using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Dashboard;
using PrimePOS.DAL.Interfaces;

namespace PrimePOS.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository _ventaRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly IClienteRepository _clienteRepo;
        public DashboardService(IVentaRepository ventaRepo, IProductoRepository productoRepo, IClienteRepository clienteRepo)
        {
            _ventaRepo = ventaRepo;
            _productoRepo = productoRepo;
            _clienteRepo = clienteRepo;
        }
        public async Task<DashboardResumeDto> ObtenerVentasDelDiaAsync(int turnoId)
        {
            return new DashboardResumeDto
            {
                VentasDelDia = await _ventaRepo.ObtenerTotalVentasPorTurnoAsync(turnoId),
                VentasCount = await _ventaRepo.ObtenerCantidadVentasPorTurnoAsync(turnoId),
                ProductoCount = await _productoRepo.CountAsync(),
                ClienteCount = await _clienteRepo.CountAsync()
            };
        }
        public async Task<DashboardInventoryDto> ObtenerResumenInventarioAsync()
        {

            return new DashboardInventoryDto
            {
                ProductoCount = await _productoRepo.CountAsync(),
                ClienteCount = await _clienteRepo.CountAsync(),
                ProductoAgotado = await _productoRepo.CountProductosAgotadoAsync()
            };
        }
    }
}
