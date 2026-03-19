using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services
{
    public class CajaService
    {
        private readonly CajaRepository _cajaRepository;

        public CajaService(CajaRepository cajaRepository)
        {
            _cajaRepository = cajaRepository;
        }

        public async Task CrearCajaAsync(CajaDto dto)
        {
            var caja = new Caja
            {
                Nombre = dto.Nombre,
                Estado = dto.Estado
            };

            _cajaRepository.Crear(caja);

            await _cajaRepository.GuardarCambiosAsync();
        }

        public async Task<CajaDto?> ObtenerCajaPorIdAsync(int id)
        {
            var caja = await _cajaRepository.ObtenerCajaPorIdAsync(id);

            if (caja == null)
                return null;

            return new CajaDto
            {

                Nombre = caja.Nombre,
                Estado = caja.Estado,
            };
        }
        public async Task<List<CajaDto>> ListarCajasAsync()
        {
            var cajas = await _cajaRepository.ListarCajasAsync();
            return cajas.Select(c => new CajaDto
            {
                CajaId = c.CajaId,
                Nombre = c.Nombre,
                Estado = c.Estado
            }).ToList();
        }

        public async Task ActualizarCajaAsync(CajaDto dto)
        {
            var caja = await _cajaRepository.ObtenerCajaPorIdAsync(dto.CajaId)
                ?? throw new Exception("Debe seleccionar una caja");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre no puede estar vacío.");



            caja.Nombre = dto.Nombre;
            caja.Estado = dto.Estado;

            _cajaRepository.Actualizar(caja);
            await _cajaRepository.GuardarCambiosAsync();

        }

        public async Task EliminarCajaAsync(CajaDto dto)
        {
            var caja = await _cajaRepository.ObtenerCajaPorIdAsync(dto.CajaId);

            if (caja == null)
                throw new Exception("Debe de seleccionar una caja");

            _cajaRepository.Eliminar(caja);
            await _cajaRepository.GuardarCambiosAsync();

        }

    }
}
