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

        public DashboardViewModel(VentaApiService apiVenta, AppSesionViewModel sesion, NotificationService notify)
        {
            _apiVenta = apiVenta;
            _sesion = sesion;
            _notify = notify;
        }
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalVentasTexto))]
        private decimal totalVentas;


        [ObservableProperty]
        private int totalProductosVendidos;
        [ObservableProperty]
        private int totalClientesAtendidos;
        public string TotalVentasTexto => $"RD$ {TotalVentas:N2}";

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

            var response =
                await _apiVenta
                    .ObtenerTotalVentasPorTurnoAsync(
                        _sesion.TurnoActual!.TurnoId);
            if (response.Success)
            {
                TotalVentas = response.Data;
            }
            else
            {
                _notify.Warning(response.Message);
            }
        }

        public void Limpiar()
        {
            TotalVentas = 0;



            Notifications.Clear();
        }
    }
}
