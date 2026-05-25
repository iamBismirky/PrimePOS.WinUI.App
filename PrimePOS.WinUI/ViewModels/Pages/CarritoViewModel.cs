using CommunityToolkit.Mvvm.ComponentModel;

namespace PrimePOS.WinUI.ViewModels.Pages;

public partial class CarritoViewModel : ObservableObject
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Codigo { get; set; } = string.Empty;

    public decimal PrecioMinoristaBase { get; set; }
    public decimal PrecioMayoristaBase { get; set; }

    [ObservableProperty]
    private int cantidad;

    [ObservableProperty]
    private decimal precio;


    [ObservableProperty]
    private decimal itbisUnitario;

    public decimal Subtotal => Cantidad * Precio;

    public bool AplicaItbis => ItbisUnitario > 0;
    public decimal Itbis => Cantidad * ItbisUnitario;

    public decimal Total => Subtotal + Itbis;

    partial void OnCantidadChanged(int value)
    {
        NotificarTotales();
    }

    partial void OnPrecioChanged(decimal value)
    {
        NotificarTotales();
    }

    partial void OnItbisUnitarioChanged(decimal value)
    {
        NotificarTotales();
    }

    public void NotificarTotales()
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Itbis));
        OnPropertyChanged(nameof(Total));
    }
}