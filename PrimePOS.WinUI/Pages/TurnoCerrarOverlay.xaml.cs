using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TurnoCerrarOverlay : UserControl
    {
        private readonly TurnoService _turnoService;
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

                this.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);
            }
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

        }
    }
}
