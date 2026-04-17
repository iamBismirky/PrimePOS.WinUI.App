using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.Services;
using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Overlays
{

    public sealed partial class AbrirTurnoOverlay : UserControl
    {
        private readonly TurnoService _turnoService;
        private readonly CajaService _cajaService;
        private readonly AppSesionViewModel _appSesion;
        public VentaViewModel _ventaViewModel { get; }



        public int TurnoId { get; set; }
        public event Action? OnCerrar;

        public AbrirTurnoOverlay()
        {

            this.InitializeComponent();
            _turnoService = App.Services.GetRequiredService<TurnoService>();
            _cajaService = App.Services.GetRequiredService<CajaService>();
            _ventaViewModel = App.Services.GetRequiredService<VentaViewModel>();
            _appSesion = App.Services.GetRequiredService<AppSesionViewModel>();
            DataContext = _ventaViewModel;

        }
        private async void Page_loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());

            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (_ventaViewModel.AppSesion.HayTurnoAbierto)
            {
                OnCerrar?.Invoke();

            }

        }
        private async void BtnAbrir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numeroTurno = await _turnoService.ObtenerSiguienteTurno();

                var dto = new CrearTurnoDto
                {
                    CajaId = (int)cmbCajas.SelectedValue,
                    NumeroTurno = numeroTurno,
                    UsuarioId = _ventaViewModel.AppSesion.UsuarioActual!.UsuarioId,
                    MontoInicial = (decimal)nbMontoInicial.Value,


                };

                var turnoActual = await _turnoService.CrearTurnoDtoAsync(dto);
                _ventaViewModel.AppSesion.AbrirTurno(turnoActual);

                if (turnoActual != null)
                {
                    await DialogHelper.MostrarMensaje(this.XamlRoot, "Exito", "Turno abierto correctamente");

                    this.Visibility = Visibility.Collapsed;
                    OnCerrar?.Invoke();
                }

            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());
            }
        }

        public async Task CargarDatosAsync()
        {
            try
            {
                //Cajas
                var lista = await _cajaService.ListarCajasAsync();
                cmbCajas.ItemsSource = lista;
                cmbCajas.DisplayMemberPath = "Nombre";
                cmbCajas.SelectedValuePath = "CajaId";
                cmbCajas.SelectedIndex = 0;


                //Turnos
                int numeroTurno = await _turnoService.ObtenerSiguienteTurno();
                cmbTurnos.ItemsSource = new List<string> { $"Turno: {DateTime.Today:dd/MM/yyyy} - T{numeroTurno}" };
                cmbTurnos.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());

            }
        }

    }
}
