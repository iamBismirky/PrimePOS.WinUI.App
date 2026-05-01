using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.MetodoPago;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class MetodoPagoApiService : BaseApiService
{
    public MetodoPagoApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    // 🔹 LISTAR MÉTODOS DE PAGO
    public Task<ApiResponse<List<MetodoPagoDto>>> ObtenerMetodosPagoAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/metodopago");
        return SendAsync<List<MetodoPagoDto>>(request);
    }
}