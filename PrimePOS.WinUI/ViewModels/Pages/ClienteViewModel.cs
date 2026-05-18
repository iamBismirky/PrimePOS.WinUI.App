using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.Views.Overlays;
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
    public async Task NuevoAsync()
    {
        var vm = new ClienteOverlayViewModel(_apiCliente, _notify);
        await vm.InicializarAsync();
        var overlay = new ClienteOverlay(vm);
        await _overlayService.ShowClienteAsync(overlay, vm);

    }

    [RelayCommand]
    public async Task EditarAsync(ClienteDto cliente)
    {
        var vm = new ClienteOverlayViewModel(_apiCliente, _notify, cliente);
        await vm.InicializarAsync();
        var overlay = new ClienteOverlay(vm);

        var actualizado = await _overlayService
            .ShowClienteAsync(overlay, vm);

        if (actualizado)
        {
            await CargarClientesAsync();
        }

    }

    [RelayCommand]
    public async Task DesactivarAsync(ClienteDto cliente)
    {
        var confirmado = await _overlayService.ConfirmAsync(
        "Eliminar cliente",
        $"¿Seguro que deseas eliminar a {cliente.Nombre}?"
    );

        if (!confirmado)
            return;

        var res = await _apiCliente.DesactivarClienteAsync(cliente.ClienteId);

        if (res.Success)
        {
            _notify.Success(res.Message);

            await CargarClientesAsync();
        }
        else
        {
            _notify.Error(res.Message);
        }

    }






}