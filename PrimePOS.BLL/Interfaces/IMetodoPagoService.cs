using PrimePOS.Contracts.DTOs.MetodoPago;

namespace PrimePOS.BLL.Interfaces
{
    public interface IMetodoPagoService
    {
        Task<List<MetodoPagoDto>> ListarMetodosPagosAsync();
    }
}