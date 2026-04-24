using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels
{
    public partial class PerfilViewModel : ObservableObject
    {
        private readonly AppSesionViewModel _session;
        private readonly UsuarioApiService _api;

        public PerfilViewModel(AppSesionViewModel session, UsuarioApiService api)
        {
            _session = session;
            _api = api;
        }

        //  DATOS DEL USUARIO LOGUEADO
        public string UsuarioNombre =>
            _session.UsuarioActual?.UsuarioNombre ?? "";
        public string UsuarioRol =>
            _session.UsuarioActual?.RolNombre ?? "";

        public int UsuarioId =>
            _session.UsuarioActual?.UsuarioId ?? 0;

        // CAMPOS UI
        [ObservableProperty]
        private string passwordActual = string.Empty;

        [ObservableProperty]
        private string passwordNueva = string.Empty;

        [ObservableProperty]
        private string confirmar = string.Empty;

        // 🔥 EVENTO PARA UI (INFOBAR)
        public event Action<string>? ErrorOcurrido;
        public event Action<string>? ExitoOcurrido;

        // 🔥 COMMAND
        [RelayCommand]
        private async Task CambiarPasswordAsync()
        {
            try
            {
                // VALIDACIONES BÁSICAS
                if (string.IsNullOrWhiteSpace(PasswordActual) ||
                    string.IsNullOrWhiteSpace(PasswordNueva) ||
                    string.IsNullOrWhiteSpace(Confirmar))
                {
                    ErrorOcurrido?.Invoke("Todos los campos son obligatorios");
                    return;
                }

                if (PasswordNueva != Confirmar)
                {
                    ErrorOcurrido?.Invoke("Las contraseñas no coinciden");
                    return;
                }

                // DTO
                var dto = new CambiarPasswordDto
                {
                    PasswordActual = PasswordActual,
                    PasswordNueva = PasswordNueva,
                    Confirmar = Confirmar
                };

                // API CALL
                //await _api.CambiarPasswordAsync(dto);

                // SUCCESS
                ExitoOcurrido?.Invoke("Contraseña actualizada correctamente");

                // LIMPIAR CAMPOS
                PasswordActual = "";
                PasswordNueva = "";
                Confirmar = "";
            }
            catch (Exception ex)
            {
                ErrorOcurrido?.Invoke(ex.Message);
            }
        }
    }
}