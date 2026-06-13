using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Empresa;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels.Overlays;
using PrimePOS.WinUI.Views.Overlays;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Pages
{
    public partial class AdministracionViewModel : ObservableObject
    {
        private readonly NotificationService _notify;
        private readonly OverlayService _overlayService;
        private readonly EmpresaApiService _apiEmpresa;

        public AdministracionViewModel(NotificationService notify, OverlayService overlayService, EmpresaApiService apiEmpresa)
        {
            _notify = notify;
            _overlayService = overlayService;
            _apiEmpresa = apiEmpresa;
        }

        [ObservableProperty]
        private ObservableCollection<EmpresaDto> empresas = new();
        [ObservableProperty]
        private bool isLoading = false;


        [RelayCommand]
        public async Task NuevaEmpresaAsync()
        {
            var vm = App.Services.GetRequiredService<EmpresaOverlayViewModel>();
            var overlay = new EmpresaOverlay(vm);
            var creado = await _overlayService.ShowAsync(overlay, vm);
            if (!creado)
                return;


        }

        [RelayCommand]
        public async Task NuevoRolAsync()
        {
            var vm = App.Services.GetRequiredService<RolOverlayViewModel>();
            var overlay = new RolOverlay(vm);
            var creado = await _overlayService.ShowAsync(overlay, vm);
            if (!creado)
                return;
            if (creado)
            {

            }

        }
        [RelayCommand]
        public async Task NuevoUsuarioAsync()
        {
            var vm = App.Services.GetRequiredService<UsuarioOverlayViewModel>();
            var overlay = new UsuarioOverlay(vm);
            var creado = await _overlayService.ShowAsync(overlay, vm);
            if (!creado)
                return;
            if (creado)
            {
                //await CargarUsuariosAsync();
            }

        }

        [RelayCommand]
        public async Task CargarEmpresasAsync()
        {
            try
            {
                IsLoading = true;

                var res = await _apiEmpresa.ObtenerEmpresasAsync();

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al cargar empresas");
                    return;
                }

                Empresas = new ObservableCollection<EmpresaDto>(res.Data ?? new());
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
