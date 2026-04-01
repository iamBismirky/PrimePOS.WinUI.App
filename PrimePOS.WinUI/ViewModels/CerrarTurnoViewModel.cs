using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using PrimePOS.BLL.DTOs.Turno;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public class CerrarTurnoViewModel : INotifyPropertyChanged
{
    private readonly TurnoService _turnoService;
    public CerrarTurnoViewModel(TurnoService turnoService)
    {
        _turnoService = turnoService;
    }

    public CierreTurnoDto Model { get; set; } = new();

    public decimal Total => MontoInicial + TotalEfectivo + TotalTarjeta + TotalTransferencia;
    public decimal EfectivoEsperado => MontoInicial + TotalEfectivo;


    //Turno
    public int TurnoId
    {
        get => Model.TurnoId;
        set
        {
            Model.TurnoId = value;
            OnPropertyChanged();
        }
    }
    public int NumeroTurno
    {
        get => Model.NumeroTurno;
        set
        {
            Model.NumeroTurno = value;
            OnPropertyChanged();
        }
    }
    // 💰 Monto inicial
    public decimal MontoInicial
    {
        get => Model.MontoInicial;
        set
        {
            Model.MontoInicial = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EfectivoEsperado));
            OnPropertyChanged(nameof(Diferencia));


        }
    }

    // 💵 Total efectivo del sistema
    public decimal TotalEfectivo
    {
        get => Model.TotalEfectivo;
        set
        {
            Model.TotalEfectivo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EfectivoEsperado));
            OnPropertyChanged(nameof(Diferencia));
        }
    }

    // 💳 Tarjeta
    public decimal TotalTarjeta
    {
        get => Model.TotalTarjeta;
        set
        {
            Model.TotalTarjeta = value;
            OnPropertyChanged();
        }
    }

    // 🔁 Transferencia
    public decimal TotalTransferencia
    {
        get => Model.TotalTransferencia;
        set
        {
            Model.TotalTransferencia = value;
            OnPropertyChanged();
        }
    }

    public decimal TotalGeneral
    {
        get => Model.TotalGeneral;
        set
        {
            Model.TotalGeneral = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Total));
        }
    }
    // ✍️ Lo escribe el usuario
    public string EfectivoContado
    {
        get => Model.EfectivoContado.ToString();
        set
        {
            if (decimal.TryParse(value, out var resultado))
            {
                Model.EfectivoContado = resultado;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Diferencia));
                OnPropertyChanged(nameof(HayFaltante));
                OnPropertyChanged(nameof(HaySobrante));
                OnPropertyChanged(nameof(DiferenciaColor));
            }
        }
    }

    // 🔥 CALCULADO POR BLL (fuente de verdad)
    public decimal Diferencia =>
        _turnoService.CalcularDiferencia(Model);

    // 🎨 Extras visuales
    public bool HayFaltante => Diferencia < 0;
    public bool HaySobrante => Diferencia > 0;

    // Cargar datos desde BLL
    public async Task InicializarAsync(int turnoId)
    {
        var resumen = await _turnoService.ObtenerResumenTurno(turnoId);

        TurnoId = turnoId;
        NumeroTurno = Sesion.TurnoActual!.NumeroTurno;
        MontoInicial = resumen.MontoInicial;
        TotalEfectivo = resumen.TotalEfectivo;
        TotalTarjeta = resumen.TotalTarjeta;
        TotalTransferencia = resumen.TotalTransferencia;
        TotalGeneral = Total;

    }

    // 🔒 Cerrar turno
    public async Task CerrarTurnoAsync()
    {

        Model.Diferencia = Diferencia;

        await _turnoService.CerrarTurnoAsync(Model);
    }

    public SolidColorBrush DiferenciaColor
    {
        get
        {
            if (Diferencia < 0)
                return new SolidColorBrush(Colors.Red);

            if (Diferencia > 0)
                return new SolidColorBrush(Colors.Green);

            return new SolidColorBrush(Colors.Gray);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}