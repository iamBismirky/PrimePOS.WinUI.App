using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Turno;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api;

public class TurnoApiService : BaseApiService
{
    public TurnoApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
    {
    }

    // 🔹 ABRIR TURNO
    public Task<ApiResponse<TurnoDto>> AbrirTurnoAsync(CrearTurnoDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/turno/abrir")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<TurnoDto>(request);
    }

    // 🔹 CERRAR TURNO
    public Task<ApiResponse<object>> CerrarTurnoAsync(CierreTurnoDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/turno/cerrar")
        {
            Content = JsonContent.Create(dto)
        };

        return SendAsync<object>(request);
    }

    // 🔹 RESUMEN
    public Task<ApiResponse<CierreTurnoDto>> ObtenerResumenAsync(int turnoId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/turno/resumen/{turnoId}");
        return SendAsync<CierreTurnoDto>(request);
    }

    // 🔹 SIGUIENTE TURNO
    public Task<ApiResponse<int>> ObtenerSiguienteTurnoAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/turno/siguiente");
        return SendAsync<int>(request);
    }


    public Task<ApiResponse<TurnoDto>> ObtenerTurnoActivoAsync(int cajaId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/turnos/activo/{cajaId}"
        );

        return SendAsync<TurnoDto>(request);
    }
}