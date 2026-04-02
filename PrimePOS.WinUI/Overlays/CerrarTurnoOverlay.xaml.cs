using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Overlays;


public sealed partial class CerrarTurnoOverlay : UserControl
{
    private readonly TurnoService _turnoService;
    private readonly CerrarTurnoViewModel _cerrarViewModel;



    public event Action? OnCerrar;
    public CerrarTurnoOverlay()
    {

        this.InitializeComponent();
        _turnoService = App.Services.GetRequiredService<TurnoService>();
        _cerrarViewModel = App.Services.GetRequiredService<CerrarTurnoViewModel>();
        this.DataContext = _cerrarViewModel;
    }
    private async void BtnCerrarTurno_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _cerrarViewModel.CerrarTurnoAsync();

            await DialogHelper.MostrarMensaje(this.XamlRoot, "Éxito", "Turno cerrado correctamente");


        }
        catch (Exception ex)
        {
            await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
        }

    }
    private void BtnCancelar_Click(object sender, RoutedEventArgs e)
    {
        OnCerrar?.Invoke();

    }
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await CargarDatosAsync();


    }
    public async Task CargarDatosAsync()
    {
        await _cerrarViewModel.InicializarAsync();


    }

}
