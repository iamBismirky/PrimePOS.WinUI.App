using CommunityToolkit.Mvvm.ComponentModel;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.Contracts.DTOs.Turno;
using PrimePOS.Contracts.DTOs.Usuario;

namespace PrimePOS.WinUI.ViewModels
{
    public partial class AppSesionViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppSesionUsuarioDto? usuarioActual;

        [ObservableProperty]
        private TurnoDto? turnoActual;

        [ObservableProperty]
        private CajaDto? cajaActual;

        [ObservableProperty]
        private string? token;

        public int CajaId { get; set; } = 1;

        public bool HayTurnoAbierto => TurnoActual != null;

        public bool EstaAutenticado => UsuarioActual != null;

        // 🔹 Métodos de sesión

        public void IniciarSesion(AppSesionUsuarioDto usuario)
        {
            UsuarioActual = usuario;
            Token = usuario.Token;
        }

        public void CerrarSesion()
        {
            UsuarioActual = null;
            TurnoActual = null;
            Token = null;
        }

        public void AbrirCaja(CajaDto caja)
        {
            CajaActual = caja;
        }

        public void CerrarCaja()
        {
            CajaActual = null;
        }

        public void AbrirTurno(TurnoDto turno)
        {
            TurnoActual = turno;
            OnPropertyChanged(nameof(HayTurnoAbierto));
        }

        public void CerrarTurno()
        {
            TurnoActual = null;
            OnPropertyChanged(nameof(HayTurnoAbierto));
        }
    }
}