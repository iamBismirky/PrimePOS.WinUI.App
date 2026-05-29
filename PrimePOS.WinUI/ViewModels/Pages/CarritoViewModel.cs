using CommunityToolkit.Mvvm.ComponentModel;

namespace PrimePOS.WinUI.ViewModels.Pages;

public partial class CarritoViewModel : ObservableObject
{

    public int ProductoId { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public string Codigo { get; set; } = string.Empty;

    [ObservableProperty]
    private int cantidad;

    [ObservableProperty]
    private decimal precio;

    [ObservableProperty]
    private decimal itbisUnitario;

    public bool AplicaItbis => ItbisUnitario > 0;

    public decimal Subtotal => Cantidad * Precio;

    public decimal Itbis => Cantidad * ItbisUnitario;

    public decimal Total => Subtotal + Itbis;

    partial void OnCantidadChanged(int value)
    {
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Itbis));
        OnPropertyChanged(nameof(Total));
    }


}