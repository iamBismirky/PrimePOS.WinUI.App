using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace PrimePOS.WinUI.ViewModels;

public partial class CobrarViewModel : ObservableObject
{
    public decimal Total { get; }

    public CobrarViewModel(decimal total)
    {
        Total = total;
    }

    // 🔹 INPUT
    [ObservableProperty]
    private decimal efectivo;

    // 🔹 OUTPUT
    public decimal Cambio => Efectivo - Total;

    partial void OnEfectivoChanged(decimal value)
    {
        OnPropertyChanged(nameof(Cambio));
    }

    // 🔹 EVENTOS
    public event Action<CobrarViewModel>? Confirmado;
    public event Action? Cancelado;

    // 🔹 COMANDOS
    [RelayCommand]
    private void Confirmar()
    {
        if (Efectivo < Total)
            return;

        Confirmado?.Invoke(this);
    }

    [RelayCommand]
    private void Cancelar()
    {
        Cancelado?.Invoke();
    }
}