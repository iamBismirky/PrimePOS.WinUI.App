using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels
{
    public partial class PerfilViewModel : ObservableObject
    {
        private readonly AppSesionViewModel _session;
        private readonly UsuarioApiService _api;
        private readonly NotificationService _notify;

        public PerfilViewModel(AppSesionViewModel session, UsuarioApiService api, NotificationService notify)
        {
            _session = session;
            _api = api;
            _notify = notify;
        }


        public string UsuarioNombre => _session.UsuarioActual?.UsuarioNombre ?? "";
        public string UsuarioRol => _session.UsuarioActual?.RolNombre ?? "";
        public string Username => _session.UsuarioActual?.Username ?? "";

        public int UsuarioId => _session.UsuarioActual?.UsuarioId ?? 0;

        // CAMPOS UI
        [ObservableProperty]
        private string passwordActual = string.Empty;

        [ObservableProperty]
        private string passwordNueva = string.Empty;

        [ObservableProperty]
        private string confirmar = string.Empty;


        [RelayCommand]
        private async Task CambiarPasswordAsync()
        {
            try
            {
                // VALIDACIONES BÁSICAS
                //if (string.IsNullOrWhiteSpace(PasswordActual) ||
                //    string.IsNullOrWhiteSpace(PasswordNueva) ||
                //    string.IsNullOrWhiteSpace(Confirmar))
                //{
                //    _notify.Warning("Todos los campos son obligatorios");
                //    return;
                //}

                //if (PasswordNueva != Confirmar)
                //{
                //    _notify.Warning("Las contraseñas no coinciden");
                //    return;
                //}

                // DTO
                var dto = new CambiarPasswordDto
                {
                    PasswordActual = PasswordActual,
                    PasswordNueva = PasswordNueva,
                    Confirmar = Confirmar
                };


                var result = await _api.CambiarPasswordAsync(dto);
                if (result.Success)
                {
                    _notify.Success("Contraseña actualizada correctamente");
                    PasswordActual = "";
                    PasswordNueva = "";
                    Confirmar = "";
                }
                else
                {
                    _notify.Error(result.Message);
                }

            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
            }
        }
    }
}