using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
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
        public string TotalVentasTexto => $"RD$ {TotalVentas:N2}";

        [RelayCommand]
        public async Task TotalVentasAsync()
        {
            if (_sesion.HayTurnoAbierto)
            {
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

        }
    }
}
