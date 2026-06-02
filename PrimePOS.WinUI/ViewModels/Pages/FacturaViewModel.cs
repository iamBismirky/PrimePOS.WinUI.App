using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Factura;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Pages
{
    public partial class FacturaViewModel : ObservableObject
    {
        private readonly FacturaApiService _apiFactura;
        private readonly NotificationService _notify;
        private readonly OverlayService _overlayService;
        private readonly PdfViewService _pdfService;
        public FacturaViewModel(FacturaApiService apiFactura, NotificationService notify, OverlayService overlayService, PdfViewService pdfService)
        {
            _apiFactura = apiFactura;
            _notify = notify;
            _overlayService = overlayService;
            _pdfService = pdfService;
        }

        [ObservableProperty]
        private ObservableCollection<FacturaListadoDto> facturas = new();

        [ObservableProperty]
        private bool isLoading;



        [RelayCommand]
        public async Task CargarFacturasAsync()
        {
            try
            {
                IsLoading = true;

                var res = await _apiFactura.ObtenerFacturasAsync();

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al cargar facturas");
                    return;
                }


                Facturas = new ObservableCollection<FacturaListadoDto>(res.Data ?? new());
            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
