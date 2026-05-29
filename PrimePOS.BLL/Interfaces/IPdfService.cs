using PrimePOS.Contracts.DTOs.Factura;

namespace PrimePOS.BLL.Interfaces
{
    public interface IPdfService
    {
        string BuildFacturaUrl(string fileName);
        string GenerateFacturaPdf(FacturaDto factura);
        string GetFilePath(string fileName);
    }
}