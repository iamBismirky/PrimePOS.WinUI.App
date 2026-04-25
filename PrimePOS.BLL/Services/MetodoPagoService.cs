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

        //Listar metodos de pagos
        public async Task<List<MetodoPagoDto>> ListarMetodosPagosAsync()
        {
            var metodosPago = await _metodoPagoRepository.ListarMetodosPagosAsync();


            return metodosPago.Select(m => new MetodoPagoDto
            {
                MetodoPagoId = m.MetodoPagoId,
                Nombre = m.Nombre,
                Estado = m.Estado,

            }).ToList();

        }
    }
}
