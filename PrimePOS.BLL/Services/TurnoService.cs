using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.DAL.Repositories;
using PrimePOS.DAL.UnitOfWork;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class TurnoService
{
    private readonly TurnoRepository _turnoRepository;
    private readonly CajaRepository _cajaRepository;
    private readonly CierreTurnoRepository _cierreTurnoRepository;
    private readonly VentaRepository _ventaRepository;
    private readonly MetodoPagoRepository _metodoPagoRepository;
    private readonly UnitOfWork _unitOfWork;

    public TurnoService(TurnoRepository turnoRepository,
        CajaRepository cajaRepository,
        CierreTurnoRepository cierreTurnoRepository,
        VentaRepository ventaRepository,
        MetodoPagoRepository metodoPagoRepository,
        UnitOfWork unitOfWork)
    {
        _turnoRepository = turnoRepository;
        _cajaRepository = cajaRepository;
        _cierreTurnoRepository = cierreTurnoRepository;
        _ventaRepository = ventaRepository;
        _metodoPagoRepository = metodoPagoRepository;
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
            var existeCaja = await _cajaRepository.ExisteCajaIdAsync(dto.CajaId);

            if (!existeCaja)
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

    public async Task CerrarTurnoAsync(CierreTurnoDto cierre)
    {
        var turno = await _turnoRepository.ObtenerPorIdAsync(cierre.TurnoId);

        if (!turno!.EstaAbierto)
            throw new Exception("El turno ya está cerrado");

        cierre.Diferencia = cierre.EfectivoContado -
            (cierre.MontoInicial + cierre.TotalEfectivo);

        // (Opcional pero recomendado)
        var cierreEntity = new CierreTurno
        {
            TurnoId = cierre.TurnoId,
            MontoInicial = cierre.MontoInicial,
            TotalEfectivo = cierre.TotalEfectivo,
            TotalTarjeta = cierre.TotalTarjeta,
            TotalTransferencia = cierre.TotalTransferencia,
            EfectivoContado = cierre.EfectivoContado,
            Diferencia = cierre.Diferencia,
            FechaCierre = DateTime.Now
        };

        await _cierreTurnoRepository.AgregarAsync(cierreEntity);

        turno.EstaAbierto = false;
        turno.FechaCierre = DateTime.Now;

        _turnoRepository.Actualizar(turno);
        await _turnoRepository.GuardarCambiosAsync();
    }
    public async Task<CierreTurnoDto> ObtenerResumenTurno(int turnoId)
    {
        var turno = await _turnoRepository.ObtenerPorIdAsync(turnoId);

        var efectivo = await _metodoPagoRepository.ObtenerPorIdAsync(1);
        var tarjeta = await _metodoPagoRepository.ObtenerPorIdAsync(2);
        var trasnferencia = await _metodoPagoRepository.ObtenerPorIdAsync(3);

        var totalEfectivo = await _ventaRepository
            .ObtenerTotalPorMetodoPagoAsync(turnoId, efectivo!.MetodoPagoId);

        var totalTarjeta = await _ventaRepository
            .ObtenerTotalPorMetodoPagoAsync(turnoId, tarjeta!.MetodoPagoId);

        var totalTransferencia = await _ventaRepository
            .ObtenerTotalPorMetodoPagoAsync(turnoId, trasnferencia!.MetodoPagoId);

        return new CierreTurnoDto
        {
            TurnoId = turnoId,
            MontoInicial = turno!.MontoInicial,
            TotalEfectivo = totalEfectivo,
            TotalTarjeta = totalTarjeta,
            TotalTransferencia = totalTransferencia,
            TotalGeneral = totalEfectivo + totalTarjeta + totalTransferencia
        };
    }

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
        var hoy = DateTime.Today;

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
    public async Task<TurnoDto?> ObtenerTurnoAbiertoAsync(int cajaId, int usuarioId)
    {
        var turno = await _turnoRepository.ObtenerTurnoAbiertoAsync(cajaId, usuarioId);

        if (turno == null) return null;

        return new TurnoDto
        {
            TurnoId = turno.TurnoId,
            CajaId = turno.CajaId,
            CajaNombre = turno.Caja?.Nombre ?? "",
            UsuarioId = turno.UsuarioId,
            UsuarioNombre = turno.Usuario?.Nombre + " " + turno.Usuario?.Apellidos ?? "",
            RolNombre = turno.Usuario?.Rol?.Nombre ?? "",
            NumeroTurno = turno.NumeroTurno,
            FechaApertura = turno.FechaApertura,
            EstaAbierto = turno.EstaAbierto,
            MontoInicial = turno.MontoInicial
        };
    }
    public decimal CalcularDiferencia(CierreTurnoDto cierre)
    {
        return cierre.EfectivoContado -
               (cierre.MontoInicial + cierre.TotalEfectivo);
    }
}