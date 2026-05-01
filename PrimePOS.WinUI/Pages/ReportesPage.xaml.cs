using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages;

public sealed partial class ReportesPage : Page
{
    public ReporVentaViewModel ViewModel { get; }

    public ReportesPage()
    {
        InitializeComponent();
        ViewModel = App.AppServices.GetRequiredService<ReporVentaViewModel>();
        this.DataContext = ViewModel;
    }
    private void BtnLimpiarFiltros_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnGenerarReporte_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnExportarPDF_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnExportarExcel_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
