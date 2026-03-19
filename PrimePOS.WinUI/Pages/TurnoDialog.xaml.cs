using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Caja;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TurnoDialog : ContentDialog
    {

        public Caja? CajaSeleccionada { get; private set; }
        public Turno? TurnoSeleccionado { get; private set; }
        public TurnoDialog()
        {
            this.InitializeComponent();
            _ = ListarCajasAsync();
        }
        private async void dlgAbrirTurno_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var caja = new TurnoDto
            {
                //CajaId = 1,
                //UsuarioId = SesionUsuario.UsuarioId,
                MontoInicial = Convert.ToDecimal(nbMontoInicial.Value),
                //Turno = Convert.ToInt32(cmbTurno.SelectedValue),

            };


            await Servicios.TurnoService.AbrirTurnoAsync(caja);
        }

        private async Task ListarCajasAsync()
        {
            var lista = await Servicios.CajaService.ListarCajasAsync();

            cmbCajas.ItemsSource = lista;

            cmbCajas.DisplayMemberPath = "Nombre";
            cmbCajas.SelectedValuePath = "CajaId";
        }
        private async Task ListarTurnosAsync(int cajaId)
        {
            var lista = await Servicios.TurnoService.ObtenerTurnosPorCajaAsync(cajaId);

            cmbTurnos.ItemsSource = lista;

            cmbTurnos.DisplayMemberPath = "CodigoTurno";
        }
        private async void cmbCajas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var caja = (Caja)cmbCajas.SelectedItem;

            if (caja == null)
                return;

            var turnos = await Servicios.TurnoService.ObtenerTurnosPorCajaAsync(caja.CajaId);

            cmbTurnos.ItemsSource = turnos;

            // Auto seleccionar turno abierto
            var abierto = turnos.FirstOrDefault(t => t.EstaAbierto);

            if (abierto != null)
            {
                cmbTurnos.SelectedItem = abierto;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CajaSeleccionada = (Caja)cmbCajas.SelectedItem;
            TurnoSeleccionado = (Turno)cmbTurnos.SelectedItem;

            if (CajaSeleccionada == null || TurnoSeleccionado == null)
            {
                args.Cancel = true;
            }
        }
    }
}
