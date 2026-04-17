using PrimePOS.Contracts.DTOs.MetodoPago;
using PrimePOS.DAL.Repositories;

namespace PrimePOS.BLL.Services
{
    public class MetodoPagoService
    {
        private readonly MetodoPagoRepository _metodoPagoRepository;

        public MetodoPagoService(MetodoPagoRepository metodoPagoRepository)
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
