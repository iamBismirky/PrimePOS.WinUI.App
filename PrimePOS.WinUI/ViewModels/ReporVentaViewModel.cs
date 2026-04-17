using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.BLL.Services;
using PrimePOS.Contracts.DTOs.Venta;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;


public partial class ReporVentaViewModel : ObservableObject
{
    private readonly VentaService _ventaService;
    private readonly AppSesionViewModel _appSesion;

    public ReporVentaViewModel(VentaService ventaService, AppSesionViewModel appSesion)
    {
        _ventaService = ventaService;
        _appSesion = appSesion;
    }

    public ObservableCollection<VentaDto> Ventas { get; set; } = new();
    public decimal TotalVentas => Ventas.Sum(v => v.Total);

    [RelayCommand]
    public async Task CargarVentasPorTurnoAsync()
    {
        if (_appSesion.TurnoActual == null)
            return;
        var ventas = await _ventaService.ObtenerVentasPorTurnoAsync(_appSesion.TurnoActual!.TurnoId);
        Ventas.Clear();
        foreach (var venta in ventas)
        {
            Ventas.Add(venta);
        }
        OnPropertyChanged(nameof(TotalVentas));
    }
    [RelayCommand]
    public async Task CargarVentasDiaAsync()
    {
        var lista = await _ventaService.ObtenerVentasDelDiaAsync();

        Ventas.Clear();

        foreach (var item in lista)
            Ventas.Add(item);

        OnPropertyChanged(nameof(TotalVentas));
    }

}

