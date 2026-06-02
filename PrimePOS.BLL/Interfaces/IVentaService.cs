using PrimePOS.Contracts.DTOs.Venta;

namespace PrimePOS.BLL.Interfaces
{
    public interface IVentaService
    {
        Task<List<ClienteVentaDto>> BuscarClientesAsync(string texto);
        Task<List<ProductoVentaDto>> BuscarProductosAsync(string texto, int TipoPrecioId);
        Task<ClienteVentaDto> CargarConsumidorFinalAsync();

        //Task<int> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto);
        Task<VentaConFacturaDto> CrearVentaAsync(int userId, string nombre, CrearVentaDto dto);
        Task<List<VentaDto>> ObtenerVentasDelDiaAsync();
        Task<decimal> ObtenerVentasPorTurnoAsync(int turnoId);
        Task<List<ProductoVentaDto>> RecalcularProductosAsync(List<int> productosIds, int tipoPrecioId);
    }
}