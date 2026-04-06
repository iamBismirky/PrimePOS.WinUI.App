using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;
namespace PrimePOS.WinUI.ViewModels
{
    public class CobrarViewModel : ObservableObject
    {
        public decimal Total { get; set; }
        public CobrarViewModel(decimal total)
        {
            Total = total;
            EfectivoTexto = "";
        }
        private string? _efectivoTexto;
        public string? EfectivoTexto
        {
            get => _efectivoTexto;
            set
            {
                if (SetProperty(ref _efectivoTexto, value))
                {


                    OnPropertyChanged(nameof(Efectivo));
                    OnPropertyChanged(nameof(Cambio));
                    OnPropertyChanged(nameof(Falta));
                    OnPropertyChanged(nameof(EsPagoValido));
                }
            }
        }

        public decimal Efectivo
        {
            get
            {
                if (decimal.TryParse(EfectivoTexto, out var monto))
                    return monto;

                return -1;
            }
        }
        public decimal Cambio => (decimal)Efectivo >= Total ? (decimal)Efectivo - Total : 0;

        public decimal Falta => (decimal)Efectivo < Total ? Total - (decimal)Efectivo : 0;

        public bool EsPagoValido => (decimal)Efectivo >= Total;

        public event Func<CobrarViewModel, Task>? Confirmado;

        public async Task ConfirmarAsync()
        {
            //if (Efectivo == 0)
            //    throw new Exception("Ingrese un monto.");

            if (!EsPagoValido)
                throw new Exception("El monto es insuficiente.");

            if (Confirmado != null)
                await Confirmado.Invoke(this);
        }
        public SolidColorBrush FaltaColor
        {
            get
            {
                if (Falta > 0)
                    return new SolidColorBrush(Colors.Red);

                if (Falta == 0)
                    return new SolidColorBrush(Colors.Green);

                return new SolidColorBrush(Colors.Gray);
            }
        }



    }
}
