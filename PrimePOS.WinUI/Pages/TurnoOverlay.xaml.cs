using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Turno;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Pages
{

    public sealed partial class TurnoOverlay : UserControl
    {
        private readonly TurnoService _turnoService;
        private readonly CajaService _cajaService;
        public string? TextoTurnoPreview { get; set; }

        public TurnoOverlay()
        {


            this.InitializeComponent();



            _turnoService = App.Services.GetRequiredService<TurnoService>();
            _cajaService = App.Services.GetRequiredService<CajaService>();
        }
        private async void Page_loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await ListarCajasAsync();
                await ListarTurnosAsync();
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());

            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
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
                    UsuarioId = Sesion.UsuarioId,
                    MontoInicial = (decimal)nbMontoInicial.Value,


                };
                var turnoActual = await _turnoService.CrearTurnoDtoAsync(dto);

                Sesion.UsuarioId = turnoActual.UsuarioId;
                Sesion.TurnoId = turnoActual.TurnoId;
                Sesion.CajaId = turnoActual.CajaId;
                Sesion.TurnoActual = turnoActual;
                if (turnoActual != null)
                {
                    await DialogHelper.MostrarMensaje(this.XamlRoot, "Exito", "Turno abierto correctamente");

                    this.Visibility = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());
            }
        }

        private async Task ListarCajasAsync()
        {
            try
            {
                var lista = await _cajaService.ListarCajasAsync();

                cmbCajas.ItemsSource = lista;

                cmbCajas.DisplayMemberPath = "Nombre";
                cmbCajas.SelectedValuePath = "CajaId";
                cmbCajas.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());

            }
        }
        private async Task ListarTurnosAsync()
        {
            try
            {
                int numeroTurno = await _turnoService.ObtenerSiguienteTurno();
                var textoTurno = $"Turno: {DateTime.Today:dd/MM/yyyy} - T{numeroTurno}";
                var TextoTurnoPreview = new List<string> { textoTurno };
                cmbTurnos.ItemsSource = TextoTurnoPreview;
                cmbTurnos.SelectedValuePath = "numeroTurno";
            }
            catch (Exception ex)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.ToString());
            }
        }
    }
}
