using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels
{
    public partial class ClienteOverlayViewModel : ObservableObject
    {
        private readonly ClienteApiService _api;
        private readonly NotificationService _notify;
        private readonly OverlayService _overlayService;
        private Func<Task>? _onSaved;

        public ClienteOverlayViewModel(
            ClienteApiService api,
            NotificationService notify,
            OverlayService overlayService)
        {
            _api = api;
            _notify = notify;
            _overlayService = overlayService;
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

        public void InitNuevo(Func<Task> onSaved)
        {
            Cliente = null;
            _onSaved = onSaved;
            Limpiar();
        }

        public void InitEditar(ClienteDto c, Func<Task> onSaved)
        {
            _onSaved = onSaved;

            Cliente = c;
            Codigo = c.Codigo;
            Nombre = c.Nombre;
            Documento = c.Documento;
            Email = c.Email;
            Telefono = c.Telefono;
            Direccion = c.Direccion;
            Estado = c.Estado;
            FechaRegistro = c.FechaRegistro;
        }

        [RelayCommand]
        public async Task GuardarAsync()
        {
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
                if (result.Success == false)
                {
                    _notify.Error(result.Message ?? "Error al crear cliente");
                    return;
                }
                _notify.Success(result.Message ?? "Cliente creado");
            }
            else
            {
                Cliente.Nombre = Nombre;
                Cliente.Documento = Documento;
                Cliente.Email = Email;
                Cliente.Telefono = Telefono;
                Cliente.Direccion = Direccion;
                Cliente.Estado = Estado;

                var result = await _api.ActualizarClienteAsync(Cliente.ClienteId, Cliente);

                if (result.Success == false)
                {
                    _notify.Error(result.Message ?? "Error al actualizar cliente");
                    return;
                }
                _notify.Success(result.Message ?? "Cliente actualizado");
            }

            if (_onSaved != null)
            {
                await _onSaved();
            }

            Limpiar();
            _overlayService.Close();
        }

        [RelayCommand]
        public void Cancelar()
        {
            Limpiar();
            _overlayService.Close();
        }
        public void Limpiar()
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
}
