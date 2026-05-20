using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class ClienteOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly ClienteApiService _api;
    private readonly NotificationService _notify;

    private readonly TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public ClienteOverlayViewModel(
        ClienteApiService api,
        NotificationService notify,
        ClienteDto? cliente = null)
    {
        _api = api;
        _notify = notify;

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
        }
    }

    [ObservableProperty] private ClienteDto? cliente;

    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string codigo = "";
    [ObservableProperty] private string documento = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string telefono = "";
    [ObservableProperty] private string direccion = "";
    [ObservableProperty] private bool estado = true;
    [ObservableProperty] private DateTime fechaRegistro = DateTime.Today;
    [ObservableProperty] private bool isLoading = false;


    [RelayCommand]
    private async Task GuardarAsync()
    {
        try
        {
            IsLoading = true;

            if (Cliente == null)
            {
                var result = await _api.CrearClienteAsync(new ClienteDto
                {
                    Nombre = Nombre,
                    Documento = Documento,
                    Email = Email,
                    Telefono = Telefono,
                    Direccion = Direccion,
                    Estado = Estado
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
                Cliente.Nombre = Nombre;
                Cliente.Documento = Documento;
                Cliente.Email = Email;
                Cliente.Telefono = Telefono;
                Cliente.Direccion = Direccion;
                Cliente.Estado = Estado;

                var result = await _api
                    .ActualizarClienteAsync(
                        Cliente.ClienteId,
                        Cliente);

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