using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Overlays;
using PrimePOS.WinUI.ViewModels;
using System;

namespace PrimePOS.WinUI.Pages;

public sealed partial class VentasPage : Page
{
    private readonly VentaViewModel _vm;
    private readonly IServiceProvider _sp;

    public VentasPage()
    {
        InitializeComponent();

        _sp = App.AppServices;

        _vm = _sp.GetRequiredService<VentaViewModel>();

        // 🔥 Conectar overlays
        _vm.MostrarOverlay = MostrarOverlay;
        _vm.CerrarOverlay = CerrarOverlay;

        this.DataContext = _vm;
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        await _vm.InicializarAsync();
    }

    // =========================
    // 🔥 OVERLAY ENGINE
    // =========================

    private void MostrarOverlay(object vm)
    {
        OverlayContainer.Children.Clear();

        var view = CrearVista(vm);

        OverlayContainer.Children.Add(view);
        OverlayContainer.Visibility = Visibility.Visible;
    }

    private void CerrarOverlay()
    {
        OverlayContainer.Children.Clear();
        OverlayContainer.Visibility = Visibility.Collapsed;
    }

    // 🔥 FACTORY DE VISTAS (AQUÍ SE CONECTA TODO)
    private UIElement CrearVista(object vm)
    {
        switch (vm)
        {
            // =========================
            // 🔓 ABRIR TURNO
            // =========================
            case AbrirTurnoViewModel abrir:
                {
                    var view = new AbrirTurnoOverlay
                    {
                        DataContext = abrir
                    };

                    abrir.OnCerrar += CerrarOverlay;

                    return view;
                }

            // =========================
            // 🔒 CERRAR TURNO
            // =========================
            case CerrarTurnoViewModel cerrar:
                {
                    var view = new CerrarTurnoOverlay
                    {
                        DataContext = cerrar
                    };

                    cerrar.OnCerrar += CerrarOverlay;

                    return view;
                }

            // =========================
            // 💰 COBRAR
            // =========================
            case CobrarViewModel cobrar:
                {
                    var view = new CobrarOverlay
                    {
                        DataContext = cobrar
                    };

                    cobrar.Cancelado += CerrarOverlay;

                    //cobrar.Confirmado += async (c) =>
                    //{
                    //    try
                    //    {
                    //        await _vm.FacturarDesdeCobroAsync(c.Efectivo, c.Cambio);
                    //        CerrarOverlay();
                    //    }
                    //    catch (Exception)
                    //    {
                    //        // ya NotificationService maneja error
                    //    }
                    //};

                    return view;
                }
        }

        throw new Exception("No hay vista para este ViewModel");
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
        btn.DataContext is CarritoItemViewModel item &&
        DataContext is VentaViewModel vm)
        {
            vm.EliminarProductoCommand.Execute(item);
        }
    }
}