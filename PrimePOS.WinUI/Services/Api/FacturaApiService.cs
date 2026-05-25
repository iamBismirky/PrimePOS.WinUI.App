using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Factura;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class FacturaApiService : BaseApiService
{
    public FacturaApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    // 🔹 GENERAR FACTURA
    public Task<ApiResponse<FacturaGeneradaDto>> GenerarFacturaAsync(int ventaId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/factura/generar/{ventaId}");

        return SendAsync<FacturaGeneradaDto>(request);
    }

    // 🔹 ANULAR FACTURA
    public Task<ApiResponse<object>> AnularFacturaAsync(int facturaId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Patch,
            $"api/factura/anular/{facturaId}");

        return SendAsync<object>(request);
    }
}