using PrimePOS.BLL.DTOs.Turno;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrimePOS.WinUI.Infrastructure
{
    public class SesionService : INotifyPropertyChanged
    {
        private TurnoDto? _turnoActual;
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
