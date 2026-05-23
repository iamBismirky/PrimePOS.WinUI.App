using PrimePOS.Contracts.DTOs.MetodoPago;

namespace PrimePOS.Contracts.DTOs.Catalog
{
    public class CatalogDto
    {
        public List<TipoClienteDto> TiposClientes { get; set; } = [];
        public List<TipoVentaDto> TiposVentas { get; set; } = [];
        public List<EstadoVentaDto> EstadosVentas { get; set; } = [];
        public List<MetodoPagoDto> MetodosPagos { get; set; } = [];
    }
}
