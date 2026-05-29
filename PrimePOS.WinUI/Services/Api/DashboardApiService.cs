using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Dashboard;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api
{
    public class DashboardApiService : BaseApiService
    {
        public DashboardApiService(IHttpClientFactory factory)
            : base(factory.CreateClient("ApiClient"))
        {
        }


        public Task<ApiResponse<DashboardResumeDto>> ObtenerTotalVentasPorTurnoAsync(int turnoId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/dashboard/turno/{turnoId}/resumen");

            return SendAsync<DashboardResumeDto>(request);
        }
        public Task<ApiResponse<DashboardInventoryDto>> ObtenerResumenInventarioAsync()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/dashboard/inventario/");

            return SendAsync<DashboardInventoryDto>(request);
        }


    }
}
