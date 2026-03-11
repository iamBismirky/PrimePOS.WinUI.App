using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI.Infrastructure;
using PrimePOS.WinUI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class VentasPage : Page
{
    public VentasViewModel Vm { get; set; } = new();
    public VentasPage()
    {
        InitializeComponent();
        DataContext = Vm;
    }
    private void BtnBorrarProducto_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnGenerarFactura_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private async void BtnAgregarProductoCarrito_Click(object sender, RoutedEventArgs e)
    {
        var producto = await Servicios.ProductoService
                .BuscarProductoCodigoONombreAsync(txtBuscarProducto.Text);

        if (producto != null)
        {
            Vm.AgregarProducto(producto);
            txtBuscarProducto.Text = "";
        }


    }
    
    private async void txtBuscarProducto_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            var producto = await Servicios.ProductoService
                .BuscarProductoCodigoONombreAsync(txtBuscarProducto.Text);

            if (producto != null)
            {
                Vm.AgregarProducto(producto);
                txtBuscarProducto.Text = "";
            }
        }

    }
    private async void txtBuscarProducto_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        
        //if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        //{
        //    var lista = await Servicios.ProductoService.BuscarProductoCodigoONombreListAsync(sender.Text);

        //    sender.ItemsSource = lista;
        //}
    }
    private void txtBuscarProducto_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        ProductoDto? producto = null;

        if (args.ChosenSuggestion != null)
        {
            producto = (ProductoDto)args.ChosenSuggestion;
        }

        if (producto != null)
        {
            Vm.AgregarProducto(producto);

            sender.Text = "";
            sender.ItemsSource = null;
        }
    }
}
