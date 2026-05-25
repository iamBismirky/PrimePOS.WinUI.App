using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Catalog;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class ClienteOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly ClienteApiService _api;
    private readonly CatalogApiService _apiTiposClientes;
    private readonly NotificationService _notify;

    private readonly TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public ClienteOverlayViewModel(
        ClienteApiService api,
        CatalogApiService apiTiposClientes,
        NotificationService notify)
    {
        _api = api;
        _apiTiposClientes = apiTiposClientes;
        _notify = notify;

    }

    [ObservableProperty] private ClienteDto? cliente;
    [ObservableProperty] private ObservableCollection<TipoClienteDto> tipoClientes = new();
    [ObservableProperty] private TipoClienteDto? tipoClienteSeleccionado;

    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string codigo = "---------";
    [ObservableProperty] private string documento = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string telefono = "";
    [ObservableProperty] private string direccion = "";
    [ObservableProperty] private bool estado = true;
    [ObservableProperty] private DateTime fechaRegistro = DateTime.Today;
    [ObservableProperty] private bool isLoading = false;


    public async Task InicializeAsync(ClienteDto? cliente)
    {
        await CargarTipoClientesAsync();
        if (cliente != null)
        {
            Cliente = cliente;

            Codigo = cliente.Codigo;
            Nombre = cliente.Nombre;
            Documento = cliente.Documento;
            Email = cliente.Email;
            Telefono = cliente.Telefono;
            Direccion = cliente.Direccion;
            Estado = cliente.Estado;
            FechaRegistro = cliente.FechaRegistro;

            TipoClienteSeleccionado =
                TipoClientes.FirstOrDefault(x =>
                    x.TipoClienteId == cliente.TipoClienteId);
        }
    }
    public async Task CargarTipoClientesAsync()
    {
        try
        {
            IsLoading = true;
            var result = await _apiTiposClientes.ObtenerTodosTipoClientesAsync();
            if (!result.Success)
            {
                _notify.Error(
                    result.Message ?? "Error al cargar tipos de clientes");
                return;
            }
            TipoClientes = new ObservableCollection<TipoClienteDto>(result.Data ?? new());
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
    private async Task GuardarAsync()
    {
        try
        {
            IsLoading = true;

            if (String.IsNullOrEmpty(Nombre))
            {
                _notify.Error("El nombre es requerido");
                return;
            }

            if (Cliente == null)
            {
                var result = await _api.CrearClienteAsync(new CrearClienteDto
                {
                    Nombre = Nombre,
                    Documento = Documento,
                    Email = Email,
                    Telefono = Telefono,
                    Direccion = Direccion,
                    Estado = Estado,
                    TipoClienteId = TipoClienteSeleccionado!.TipoClienteId

                });

                if (!result.Success)
                {
                    _notify.Error(
                        result.Message ?? "Error al crear cliente");

                    return;
                }

                _notify.Success(
                    result.Message ?? "Cliente creado");
            }
            else
            {
                var dto = new ActualizarClienteDto
                {
                    ClienteId = Cliente.ClienteId,
                    Nombre = Nombre,
                    Documento = Documento,
                    Email = Email,
                    Telefono = Telefono,
                    Direccion = Direccion,
                    Estado = Estado,
                    TipoClienteId = TipoClienteSeleccionado!.TipoClienteId
                };
                Cliente.Nombre = Nombre;
                Cliente.Documento = Documento;
                Cliente.Email = Email;
                Cliente.Telefono = Telefono;
                Cliente.Direccion = Direccion;
                Cliente.Estado = Estado;
                Cliente.TipoClienteId = TipoClienteSeleccionado!.TipoClienteId;

                var result = await _api.ActualizarClienteAsync(Cliente.ClienteId, dto);

                if (!result.Success)
                {
                    _notify.Error(
                        result.Message ?? "Error al actualizar cliente");

                    return;
                }

                _notify.Success(
                    result.Message ?? "Cliente actualizado");
            }

            Limpiar();

            Close(true);
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
    private void Cancelar()
    {
        Limpiar();

        Close(false);
    }
    public void Close(bool result = false)
    {
        _tcs.TrySetResult(result);
    }
    private void Limpiar()
    {
        Cliente = null;

        Codigo = "";
        Nombre = "";
        Documento = "";
        Telefono = "";
        Email = "";
        Direccion = "";

        FechaRegistro = DateTime.Now;

        Estado = true;
    }
}