using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.BLL.DTOs.Turno;
using PrimePOS.DAL.Repositories;
using PrimePOS.DAL.UnitOfWork;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class TurnoService
{
    private readonly TurnoRepository _turnoRepository;
    private readonly CajaRepository _cajaRepository;
    private readonly UnitOfWork _unitOfWork;

    public TurnoService(TurnoRepository turnoRepository, CajaRepository cajaRepository, UnitOfWork unitOfWork)
    {
        _turnoRepository = turnoRepository;
        _cajaRepository = cajaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TurnoDto> CrearTurnoDtoAsync(CrearTurnoDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var ahora = DateTime.Now;

            //  Validar que no haya turno abierto en la caja
            var existe = await _turnoRepository.ExisteTurnoAbierto(dto.CajaId);

            if (existe)
                throw new Exception("Ya hay un turno abierto en esta caja.");

            // Validar que la caja exista
            var existeCaja = await _cajaRepository.ExisteCajaAsync(dto.CajaId);

            if (existeCaja)
                throw new Exception("La caja no existe.");

            int numeroTurno = await ObtenerSiguienteTurno();
            // 🧱 Crear entidad
            var turno = new Turno
            {
                CajaId = dto.CajaId,
                UsuarioId = dto.UsuarioId,
                FechaApertura = ahora,
                FechaOperacion = ahora,
                NumeroTurno = numeroTurno,
                MontoInicial = dto.MontoInicial,
                EstaAbierto = true
            };

            // 💾 Guardar
            await _turnoRepository.AgregarAsync(turno);
            await _turnoRepository.GuardarCambiosAsync();

            await _unitOfWork.CommitAsync();

            // 🔄 Convertir a DTO
            return new TurnoDto
            {
                TurnoId = turno.TurnoId,
                CajaId = turno.CajaId,
                NumeroTurno = turno.NumeroTurno,
                UsuarioId = turno.UsuarioId,
                FechaApertura = turno.FechaApertura,
                MontoInicial = turno.MontoInicial,
                EstaAbierto = turno.EstaAbierto
            };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    //public async Task<int> GenerarCodigoTurno(int cajaId, DateTime fechaBase)
    //{
    //    //var hoy = fechaBase.Date;

    //    //// 🔍 Obtener último turno del día desde repositorio
    //    //var ultimo = await _turnoRepository.ObtenerUltimoTurnoDelDiaAsync(cajaId, hoy);

    //    //int correlativo = 1;

    //    //if (ultimo != null && !string.IsNullOrEmpty(ultimo.NumeroTurno))
    //    //{
    //    //    var partes = ultimo.NumeroTurno.Split('-');

    //    //    if (partes.Length == 2 && int.TryParse(partes[1], out int numero))
    //    //    {
    //    //        correlativo = numero + 1;
    //    //    }
    //    //}

    //    //string fecha = hoy.ToString("yyyyMMdd");
    //    //string numeroFormateado = correlativo.ToString("D3");

    //    //return $"{fecha}-{numeroFormateado}";
    //}
    public async Task<List<Turno>> ObtenerTurnosPorCajaAsync(int cajaId)
    {
        return await _turnoRepository.ObtenerPorCajaAsync(cajaId);
    }

    //public async Task<(DateTime fecha, int numeroTurno)> ObtenerSiguienteTurno()
    //{
    //    var hoy = DateTime.Today;

    //    var ultimoTurno = await _turnoRepository.ObtenerUltimoTurnoDelDia(hoy);

    //    int siguiente = (ultimoTurno?.NumeroTurno ?? 0) + 1;

    //    return (hoy, siguiente);
    //}
    public async Task<int> ObtenerSiguienteTurno()
    {
        var hoy = DateTime.Now;

        var ultimoTurno = await _turnoRepository
            .ObtenerUltimoTurnoDelDia(hoy);

        return (ultimoTurno?.NumeroTurno ?? 0) + 1;
    }
    public async Task<TurnoDto?> ObtenerTurnoAbiertoAsync(int usuarioId)
    {
        var turno = await _turnoRepository.ObtenerTurnoAbiertoPorUsuarioAsync(usuarioId);
        if (turno == null) return null;

        return new TurnoDto
        {
            TurnoId = turno.TurnoId,
            CajaId = turno.CajaId,
            UsuarioId = turno.UsuarioId,
            FechaApertura = turno.FechaApertura,
            NumeroTurno = turno.NumeroTurno,
            MontoInicial = turno.MontoInicial,
            FechaCierre = turno.FechaCierre,
            EstaAbierto = turno.EstaAbierto

        };
    }
    public async Task<TurnoDto?> ObtenerTurnoAbiertoPorCajaAsync(int cajaId)
    {
        var turno = await _turnoRepository.ObtenerTurnoAbiertoPorCajaAsync(cajaId);

        if (turno == null) return null;

        return new TurnoDto
        {
            TurnoId = turno.TurnoId,
            CajaId = turno.CajaId,
            UsuarioId = turno.UsuarioId,
            NumeroTurno = turno.NumeroTurno,
            FechaApertura = turno.FechaApertura,
            EstaAbierto = turno.EstaAbierto,
            MontoInicial = turno.MontoInicial
        };
    }
}