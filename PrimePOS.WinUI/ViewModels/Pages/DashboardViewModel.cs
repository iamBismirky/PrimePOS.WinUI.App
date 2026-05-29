using CommunityToolkit.Mvvm.ComponentModel;
using PrimePOS.WinUI.Models;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly VentaApiService _apiVenta;
        private readonly AppSesionViewModel _sesion;
        private readonly NotificationService _notify;
        private readonly DashboardApiService _apiDash;

        public DashboardViewModel(VentaApiService apiVenta,
            AppSesionViewModel sesion,
            NotificationService notify,
            DashboardApiService apiDash)
        {
            _apiVenta = apiVenta;
            _sesion = sesion;
            _notify = notify;
            _apiDash = apiDash;
        }
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalVentasTexto))]
        private decimal totalVentas;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalProductosTexto))]
        private int totalProductos;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalClientesTexto))]
        private int totalClientes;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalClientesTexto))]
        private int totalAgotados;
        [ObservableProperty]
        private int ventasCount;

        public string TotalVentasTexto => TotalVentas.ToString("N2");
        public string TotalProductosTexto => TotalProductos.ToString("N2");
        public string TotalClientesTexto => TotalClientes.ToString("N2");
        public string TotalAgotadosTexto => TotalAgotados.ToString("N2");

        public ObservableCollection<AppNotification> Notifications => _notify.Notifications;


        public async Task InicializarAsync()
        {
            if (!_sesion.EstaAutenticado)
                return;

            await CargarDatosAsync();
        }

        public async Task CargarDatosAsync()
        {
            if (!_sesion.EstaAutenticado ||
                !_sesion.HayTurnoAbierto)
                return;

            var respVenta = await _apiDash.ObtenerTotalVentasPorTurnoAsync(_sesion.TurnoActual!.TurnoId);
            if (!respVenta.Success)
            {
                _notify.Warning(respVenta.Message);
                return;
            }
            TotalVentas = respVenta.Data?.VentasDelDia ?? 0;
            VentasCount = respVenta.Data?.VentasCount ?? 0;


            var resuInven = await _apiDash.ObtenerResumenInventarioAsync();

            if (!resuInven.Success)
            {
                _notify.Warning(resuInven.Message);
                return;
            }
            TotalProductos = resuInven.Data?.ProductoCount ?? 0;
            TotalClientes = resuInven.Data?.ClienteCount ?? 0;
            TotalAgotados = resuInven.Data?.ProductoAgotado ?? 0;

        }

        public void Limpiar()
        {
            TotalVentas = 0;
            TotalProductos = 0;
            TotalClientes = 0;

            Notifications.Clear();
        }
    }
}
