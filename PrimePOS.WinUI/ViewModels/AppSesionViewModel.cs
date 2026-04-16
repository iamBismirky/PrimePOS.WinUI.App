
using PrimePOS.BLL.DTOs.Turno;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.Contracts.DTOs.Caja;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrimePOS.WinUI.ViewModels
{
    public class AppSesionViewModel : INotifyPropertyChanged
    {
        private AppSesionUsuarioDto? _usuarioActual;
        private TurnoDto? _turnoActual;
        private CajaDto? _cajaActual;
        public int CajaId { get; set; } = 1;

        public AppSesionUsuarioDto? UsuarioActual
        {
            get => _usuarioActual;
            set
            {
                if (_usuarioActual != value)
                {
                    _usuarioActual = value;
                    OnPropertyChanged();
                }
            }
        }
        public CajaDto? CajaActual
        {
            get => _cajaActual;
            set
            {
                if (_cajaActual != value)
                {
                    _cajaActual = value;
                    OnPropertyChanged();
                }
            }
        }
        public void AbrirCaja(CajaDto caja)
        {
            CajaActual = caja;
        }
        public void CerrarCaja()
        {
            CajaActual = null;
        }
        public void IniciarSesion(AppSesionUsuarioDto usuario)
        {
            UsuarioActual = usuario;
        }

        public void CerrarSesion()
        {
            UsuarioActual = null;
            TurnoActual = null;
        }
        public TurnoDto? TurnoActual
        {
            get => _turnoActual;
            set
            {
                if (_turnoActual != value)
                {
                    _turnoActual = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HayTurnoAbierto));
                }
            }
        }
        public bool HayTurnoAbierto => TurnoActual != null;

        public void AbrirTurno(TurnoDto turno)
        {
            TurnoActual = turno;
        }

        public void CerrarTurno()
        {
            TurnoActual = null;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
