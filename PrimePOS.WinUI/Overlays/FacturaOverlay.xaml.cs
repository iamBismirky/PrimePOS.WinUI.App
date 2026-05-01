using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;
using System;

namespace PrimePOS.WinUI.Overlays;


public sealed partial class FacturaOverlay : UserControl
{

    public FacturaOverlay()
    {

        InitializeComponent();
        this.Loaded += FacturaOverlay_Loaded;

    }
    private async void FacturaOverlay_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is FacturaViewModel vm && !string.IsNullOrEmpty(vm.UrlPdf))
            {
                await PdfViewer.EnsureCoreWebView2Async();
                PdfViewer.Source = new Uri(vm.UrlPdf);
            }
        }
        catch
        {
            // puedes usar NotificationService si quieres
        }
    }

    private void Cerrar_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is FacturaViewModel vm)
        {
            vm.Cerrar();
        }
        PdfViewer.Source = null;
    }
}
