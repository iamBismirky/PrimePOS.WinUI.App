using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Overlays
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TurnoCerrarOverlay : UserControl
    {
        private readonly TurnoService _turnoService;

        private decimal EfectivoEsperado { get; set; }
        private decimal Diferencia { get; set; }
        public int TurnoId { get; set; }
        public event Action? OnCloseRequested;
        public TurnoCerrarOverlay()
        {

            this.InitializeComponent();
            _turnoService = App.Services.GetRequiredService<TurnoService>();

        }
        private async void BtnCerrarTurno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var resumen = await _turnoService.ObtenerResumenTurno(TurnoId);

                resumen.TotalGeneral = (decimal)nbEfectivoContado.Value;

                await _turnoService.CerrarTurnoAsync(resumen);

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Éxito", "Turno cerrado correctamente");

                Sesion.TurnoActual = null;

                this.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
            }
        }
        private void nbEfectivoContado_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            var efectivoContado = (decimal)nbEfectivoContado.Value;
            txtDiferencia.Text = (efectivoContado - Diferencia).ToString();
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            OnCloseRequested?.Invoke();

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarDatosAsync();
        }
        public async Task CargarDatosAsync()
        {

            var resumen = await _turnoService.ObtenerResumenTurno(TurnoId);
            txtTurno.Text = TurnoId.ToString();

            txtTotalEfectivo.Text = resumen.TotalEfectivo.ToString("C");
            txtTotalTarjeta.Text = resumen.TotalTarjeta.ToString("C");
            txtTotalTransferencia.Text = resumen.TotalTransferencia.ToString("C");
            txtTotalGeneral.Text = resumen.TotalGeneral.ToString("C");
            txtEfectivoEsperado.Text = resumen.TotalEfectivo.ToString("C");

            Diferencia = resumen.Diferencia;

        }
    }
}
