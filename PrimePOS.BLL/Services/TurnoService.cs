using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class TurnoService
{
    private readonly TurnoRepository _turnoRepository;
    private readonly CajaRepository _cajaRepository;
    private readonly AppDbContext _context;

    public TurnoService(TurnoRepository turnoRepository, CajaRepository cajaRepository, AppDbContext context)
    {
        _turnoRepository = turnoRepository;
        _cajaRepository = cajaRepository;
        _context = context;
    }

    public async Task AbrirTurnoAsync(TurnoDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        var ahora = DateTime.Now;
        try
        {
            // Validar que no haya turno abierto
            var turnoAbierto = await _turnoRepository.ObtenerTurnoAbiertoAsync(dto.CajaId);

            if (turnoAbierto)
                throw new Exception("Ya hay un turno abierto en esta caja.");

            // Validar que la caja exista
            var existeCaja = await _cajaRepository.ExisteCajaAsync(dto.CajaId);

            if (existeCaja)
                throw new Exception("La caja no existe.");

            var codigo = await GenerarCodigoTurno(dto.CajaId, ahora);

            // Crear apertura
            var turno = new Turno
            {
                CajaId = dto.CajaId,
                CodigoTurno = codigo,
                UsuarioId = dto.UsuarioId,
                FechaApertura = ahora,
                MontoInicial = dto.MontoInicial,
                EstaAbierto = true,
            };

            await _turnoRepository.AgregarAsync(turno);
            await _turnoRepository.GuardarCambiosAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task<string> GenerarCodigoTurno(int cajaId, DateTime fechaBase)
    {
        var hoy = fechaBase.Date;

        // 🔍 Obtener último turno del día desde repositorio
        var ultimo = await _turnoRepository.ObtenerUltimoTurnoDelDiaAsync(cajaId, hoy);

        int correlativo = 1;

        if (ultimo != null && !string.IsNullOrEmpty(ultimo.CodigoTurno))
        {
            var partes = ultimo.CodigoTurno.Split('-');

            if (partes.Length == 2 && int.TryParse(partes[1], out int numero))
            {
                correlativo = numero + 1;
            }
        }

        string fecha = hoy.ToString("yyyyMMdd");
        string numeroFormateado = correlativo.ToString("D3");

        return $"{fecha}-{numeroFormateado}";
    }
    public async Task<List<Turno>> ObtenerTurnosPorCajaAsync(int cajaId)
    {
        return await _turnoRepository.ObtenerPorCajaAsync(cajaId);
    }
}