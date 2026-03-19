using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services
{
    public class AperturaCajaService
    {
        private readonly AperturaCajaRepository _aperturaCajaRepository;

        public AperturaCajaService(AperturaCajaRepository aperturaCajaRepository)
        {
            _aperturaCajaRepository = aperturaCajaRepository;
        }

        public async Task AbrirCajaAsync(AperturaCajaDto dto)
        {
            var caja = new AperturaCaja
            {
                CajaId = 1,
                UsuarioId = dto.UsuarioId,
                FechaApertura = DateTime.Now,
                MontoInicial = dto.MontoInicial,
                Turno = dto.Turno,
                Estado = true,


            };

            _aperturaCajaRepository.AbrirCaja(caja);
            await _aperturaCajaRepository.GuardarCambiosAsync();
        }
    }
}
