using System.ComponentModel;

namespace PrimePOS.BLL.DTOs.Venta
{
    public class CarritoItem : INotifyPropertyChanged
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } =  string.Empty;

        public decimal Precio { get; set; }

        public int _cantidad { get; set; }
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cantidad)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
            }
        }

        

        public decimal Total => Precio * Cantidad;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propiedad)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }
    }
}