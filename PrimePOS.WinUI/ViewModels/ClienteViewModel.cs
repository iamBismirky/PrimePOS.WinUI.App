using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Overlays;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class ClienteViewModel : ObservableObject
{
    private readonly ClienteApiService _apiCliente;
    private readonly NotificationService _notify;
    private readonly OverlayService _overlayService;

    public ClienteViewModel(ClienteApiService apiCategoria, NotificationService notify, OverlayService overlay)
    {
        _apiCliente = apiCategoria;
        _notify = notify;
        _overlayService = overlay;
    }



    [ObservableProperty] private ObservableCollection<ClienteDto> clientes = new();
    [ObservableProperty] private ObservableCollection<RolDto> roles = new();
    [ObservableProperty] private ClienteDto? clienteSeleccionado;
    [ObservableProperty] private string buscar = "";
    [ObservableProperty] private bool isLoading;


    private List<ClienteDto> _cache = new();


    public Visibility LoadingVisibility =>
        IsLoading ? Visibility.Visible : Visibility.Collapsed;

    partial void OnIsLoadingChanged(bool value)
    {
        OnPropertyChanged(nameof(LoadingVisibility));
    }

    [RelayCommand]
    public async Task CargarClientesAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _apiCliente.ObtenerClientesAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message ?? "Error al cargar clientes");
                return;
            }

            _cache = res.Data ?? new List<ClienteDto>();
            Clientes = new ObservableCollection<ClienteDto>(res.Data ?? new());
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



    [RelayCommand]
    public async Task EditarAsync(ClienteDto cliente)
    {
        var vm = App.AppServices.GetRequiredService<ClienteOverlayViewModel>();

        vm.InitEditar(cliente, async () =>
        { await CargarClientesAsync(); });

        var overlay = new ClienteOverlay(vm);
        _overlayService.Show(overlay);

    }

    [RelayCommand]
    public async Task DesactivarAsync(ClienteDto cliente)
    {
        var vm = App.AppServices
        .GetRequiredService<DialogViewModel>();

        vm.Initialize(
            "Eliminar cliente",
            $"¿Seguro que deseas eliminar a {cliente.Nombre}?",
            async () =>
            {
                var res = await _apiCliente
                    .DesactivarClienteAsync(cliente.ClienteId);

                if (res.Success)
                {
                    _notify.Success("Cliente eliminado");

                    await CargarClientesAsync();
                }
                else
                {
                    _notify.Error(res.Message);
                }

                _overlayService.Close();

            },
         _overlayService.Close);


        var view = new DialogOverlay(vm);

        _overlayService.Show(view);

    }

    [RelayCommand]
    public void Filtrar()
    {
        if (string.IsNullOrWhiteSpace(Buscar))
        {
            Clientes = new ObservableCollection<ClienteDto>(_cache);
            return;
        }

        var filtradas = _cache
            .Where(x => x.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Clientes = new ObservableCollection<ClienteDto>(filtradas);
    }

    [RelayCommand]
    public async Task NuevoAsync()
    {
        var vm = App.AppServices.GetRequiredService<ClienteOverlayViewModel>();
        vm.InitNuevo(async () => { await CargarClientesAsync(); });
        var overlay = new ClienteOverlay(vm);
        _overlayService.Show(overlay);

    }


}