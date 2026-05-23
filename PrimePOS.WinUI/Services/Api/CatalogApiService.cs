using PrimePOS.Contracts.Common;
using PrimePOS.Contracts.DTOs.Catalog;
using PrimePOS.Contracts.DTOs.MetodoPago;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services.Api
{
    public class CatalogApiService : BaseApiService
    {
        public CatalogApiService(IHttpClientFactory factory)
        : base(factory.CreateClient("ApiClient"))
        {
        }

        public Task<ApiResponse<List<TipoClienteDto>>> ObtenerTodosTipoClientesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/catalog/tipos-clientes");
            return SendAsync<List<TipoClienteDto>>(request);
        }
        public Task<ApiResponse<List<TipoVentaDto>>> ObtenerTodosTipoVentasAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/catalog/tipos-ventas");
            return SendAsync<List<TipoVentaDto>>(request);
        }
        public Task<ApiResponse<List<EstadoVentaDto>>> ObtenerTodosEstadosVentasAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/catalog/estados-ventas");
            return SendAsync<List<EstadoVentaDto>>(request);
        }
        public Task<ApiResponse<List<MetodoPagoDto>>> ObtenerTodosMetodoPagosAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/catalog/metodos-pagos");
            return SendAsync<List<MetodoPagoDto>>(request);
        }
    }
}
