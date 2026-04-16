using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services
{
    public class CajaService : ICajaService
    {
        private readonly ICajaRepository _cajaRepository;

        public CajaService(ICajaRepository cajaRepository)
        {
            _cajaRepository = cajaRepository;
        }

        public async Task CrearCajaAsync(CajaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre no puede estar vacío.", "REQUIRED_NAME");

            var existeCaja = await _cajaRepository.ObtenerPorNombreAsync(dto.Nombre);

            if (existeCaja != null)
            {

                if (!existeCaja.Estado)
                {
                    existeCaja.Estado = true;
                    _cajaRepository.Actualizar(existeCaja);
                    await _cajaRepository.GuardarCambiosAsync();
                    return;
                }
                throw new BusinessException("Ya existe una caja con este nombre.", "DUPLICATE");

            }
            else
            {
                var caja = new Caja
                {
                    Nombre = dto.Nombre,
                    Estado = true
                };

                _cajaRepository.Crear(caja);

                await _cajaRepository.GuardarCambiosAsync();
            }

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
                ?? throw new BusinessException("Debe seleccionar una caja", "REQUIRED_NOFOUND");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre no puede estar vacío.", "REQUIRED_NAME");



            caja.Nombre = dto.Nombre;
            caja.Estado = dto.Estado;

            _cajaRepository.Actualizar(caja);
            await _cajaRepository.GuardarCambiosAsync();

        }

        public async Task EliminarCajaAsync(int cajaId)
        {
            var caja = await _cajaRepository.ObtenerCajaPorIdAsync(cajaId);

            if (caja == null)
                throw new BusinessException("Debe de seleccionar una caja", "REQUIRED_NOFOUND");

            _cajaRepository.Eliminar(caja);
            await _cajaRepository.GuardarCambiosAsync();

        }
        public async Task DesactivarCajaAsync(int cajaId)
        {
            var caja = await _cajaRepository.ObtenerCajaPorIdAsync(cajaId);

            if (caja == null)
                throw new BusinessException("Debe de seleccionar una caja", "REQUIRED_NOFOUND");

            caja.Estado = false;
            _cajaRepository.Actualizar(caja);
            await _cajaRepository.GuardarCambiosAsync();

        }

    }
}
