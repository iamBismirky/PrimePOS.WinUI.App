using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.DAL.Interfaces;

namespace PrimePOS.BLL.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly IMetodoPagoRepository _metodoPagoRepository;

        public MetodoPagoService(IMetodoPagoRepository metodoPagoRepository)
        {
            _metodoPagoRepository = metodoPagoRepository;
        }


        public async Task<List<MetodoPagoDto>> ListarMetodosPagosAsync()
        {
            var metodosPago = await _metodoPagoRepository.ListarMetodosPagosAsync();
            if (metodosPago == null) throw new BusinessException("Error al obtener los métodos de pago", StatusCodes.Status404NotFound);

            return metodosPago.Select(m => new MetodoPagoDto
            {
                MetodoPagoId = m.MetodoPagoId,
                Nombre = m.Nombre,
                Estado = m.Estado,

            }).ToList();

        }
    }
}
