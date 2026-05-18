using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.ViewModels;
using PrimePOS.WinUI.ViewModels.Pages;
using System;

namespace PrimePOS.WinUI.Views.Pages;

public sealed partial class VentasPage : Page
{
    private readonly VentaViewModel _vm;
    private readonly IServiceProvider _sp;

    public VentasPage()
    {
        InitializeComponent();

        _sp = App.Services;

        _vm = _sp.GetRequiredService<VentaViewModel>();


        this.DataContext = _vm;
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        await _vm.InicializarAsync();
    }

    private async void txtBuscarProducto_TextChanged(
    AutoSuggestBox sender,
    AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            await _vm.BuscarProductosAsync();
        }
    }
    private async void txtBuscarProducto_QuerySubmitted(
    AutoSuggestBox sender,
    AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is ProductoDto producto)
        {
            await _vm.SeleccionarProductoAsync(producto);
        }
    }
    private async void txtBuscarCliente_TextChanged(
    AutoSuggestBox sender,
    AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            await _vm.BuscarClientesAsync();
        }
    }
    private async void txtBuscarCliente_QuerySubmitted(
    AutoSuggestBox sender,
    AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is ClienteDto cliente)
        {
            _vm.SeleccionarCliente(cliente);
        }
    }
    private async void txtBuscarCliente_SuggestionChosen(
    AutoSuggestBox sender,
    AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is ClienteDto cliente)
        {
            _vm.SeleccionarCliente(cliente);
        }
    }
    private void Eliminar_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn &&
        btn.DataContext is CarritoViewModel item &&
        DataContext is VentaViewModel vm)
        {
            vm.EliminarProductoCommand.Execute(item);
        }
    }
}