using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Venta;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class CobrarViewModel : ObservableObject
{
    public decimal Total { get; }

    public CobrarViewModel(decimal total)
    {
        Total = total;
        EfectivoTexto = MoneyHelper.ToString(0);
    }

    // 🔹 INPUT
    [ObservableProperty] private decimal efectivo;
    [ObservableProperty] private bool isLoading;
    private string efectivoTexto = "";
    public string EfectivoTexto
    {
        get => efectivoTexto;
        set
        {
            if (SetProperty(ref efectivoTexto, value))
            {
                Efectivo = MoneyHelper.ToDecimal(value);
            }
        }
    }
    // 🔹 OUTPUT
    public decimal Cambio => Efectivo - Total;

    partial void OnEfectivoChanged(decimal value)
    {
        OnPropertyChanged(nameof(Cambio));
    }

    // 🔹 EVENTOS
    public Func<CobroResult, Task>? ConfirmadoAsync;
    public event Action? Cancelado;

    // 🔹 COMANDOS


    [RelayCommand]
    private async Task ConfirmarAsync()
    {
        if (Efectivo < Total)
            return;

        if (ConfirmadoAsync == null)
            return;

        try
        {
            IsLoading = true;

            await ConfirmadoAsync.Invoke(new CobroResult
            {
                Efectivo = Efectivo,
                Cambio = Cambio
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Cancelar()
    {
        Cancelado?.Invoke();
    }
    public void FormatearEfectivo()
    {
        EfectivoTexto = MoneyHelper.ToString(Efectivo);
    }
}