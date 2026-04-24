using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Rol;
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
    private readonly ClienteApiService _apiCategoria;
    private readonly NotificationService _notify;

    public ClienteViewModel(ClienteApiService apiCategoria, NotificationService notify)
    {
        _apiCategoria = apiCategoria;
        _notify = notify;
    }



    [ObservableProperty] private ObservableCollection<ClienteDto> clientes = new();
    [ObservableProperty] private ObservableCollection<RolDto> roles = new();
    [ObservableProperty] private ClienteDto? clienteSeleccionado;
    [ObservableProperty] private string codigo = "";
    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string documento = "";
    [ObservableProperty] private string telefono = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string direccion = "";
    [ObservableProperty] private DateTime fechaRegistro = DateTime.Today;
    [ObservableProperty] private bool estado = true;
    [ObservableProperty] private string buscar = "";
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isOverlayVisible;


    private List<ClienteDto> _cache = new();

    public Visibility OverlayVisibility =>
        IsOverlayVisible ? Visibility.Visible : Visibility.Collapsed;

    partial void OnIsOverlayVisibleChanged(bool value)
    {
        OnPropertyChanged(nameof(OverlayVisibility));
    }

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

            var res = await _apiCategoria.ObtenerClientesAsync();

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
    public async Task GuardarAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                _notify.Warning("El nombre es obligatorio");
                return;
            }


            if (ClienteSeleccionado == null)
            {


                var res = await _apiCategoria.CrearClienteAsync(new ClienteDto
                {
                    Codigo = Codigo.Trim(),
                    Nombre = Nombre.Trim(),
                    Documento = Documento.Trim(),
                    Telefono = Telefono.Trim(),
                    Email = Email.Trim(),
                    Direccion = Direccion.Trim(),
                    Estado = Estado,
                    FechaRegistro = DateTime.Now
                });

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al crear cliente");
                    return;
                }

                _notify.Success(res.Message ?? "Cliente creado correctamente");
            }
            else
            {
                ClienteSeleccionado.Codigo = Codigo.Trim();
                ClienteSeleccionado.Nombre = Nombre.Trim();
                ClienteSeleccionado.Documento = Documento.Trim();
                ClienteSeleccionado.Telefono = Telefono.Trim();
                ClienteSeleccionado.Email = Email.Trim();
                ClienteSeleccionado.Estado = Estado;
                ClienteSeleccionado.FechaRegistro = FechaRegistro;

                var res = await _apiCategoria.ActualizarClienteAsync(
                    ClienteSeleccionado.ClienteId,
                    ClienteSeleccionado);
                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al actualizar cliente");
                    return;
                }

                _notify.Success(res.Message ?? "Cliente actualizado correctamente");
            }

            await CargarClientesAsync();
            Limpiar();
            CerrarOverlay();
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
    }

    [RelayCommand]
    public async Task EditarAsync(ClienteDto cliente)
    {
        ClienteSeleccionado = cliente;
        Codigo = cliente.Codigo;
        Nombre = cliente.Nombre;
        Estado = cliente.Estado;
        Telefono = cliente.Telefono;
        Email = cliente.Email;
        Direccion = cliente.Direccion;
        FechaRegistro = cliente.FechaRegistro;
        AbrirOverlay();
    }

    [RelayCommand]
    public async Task DesactivarAsync(ClienteDto cliente)
    {
        var res = await _apiCategoria.DesactivarClienteAsync(cliente.ClienteId);

        if (!res.Success)
        {
            _notify.Error(res.Message ?? "No se pudo desactivar");
            return;
        }

        _notify.Success(res.Message ?? "Cliente desactivado");
        await CargarClientesAsync();
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
        Limpiar();
        AbrirOverlay();
    }

    [RelayCommand]
    public void Limpiar()
    {
        Codigo = "";
        Nombre = "";
        Documento = "";
        Telefono = "";
        Email = "";
        Direccion = "";
        FechaRegistro = DateTime.Now;
        Estado = true;
        ClienteSeleccionado = null;
    }

    [RelayCommand]
    public void AbrirOverlay()
    {
        IsOverlayVisible = true;
    }

    [RelayCommand]
    public void CerrarOverlay()
    {
        IsOverlayVisible = false;
        Limpiar();
    }
}