using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrimePOS.WinUI.ViewModels;

public class CarritoItemViewModel : INotifyPropertyChanged
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;

    private decimal _cantidad;
    public decimal Cantidad
    {
        get => _cantidad;
        set
        {
            _cantidad = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Total));
        }
    }

    public decimal Precio { get; set; }

    public decimal Total => Cantidad * Precio;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}