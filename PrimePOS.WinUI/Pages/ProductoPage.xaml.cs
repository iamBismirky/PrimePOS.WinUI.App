using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.ViewModels;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class ProductoPage : Page
    {
        private readonly ProductoViewModel ViewModel;
        public ProductoPage()
        {
            InitializeComponent();
            ViewModel = App.AppServices.GetRequiredService<ProductoViewModel>();

            this.DataContext = ViewModel;

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarProductosCommand.ExecuteAsync(null);
        }

        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ProductoViewModel vm)
            {
                vm.FiltrarCommand.Execute(null);
            }

        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductoViewModel vm && sender is Button btn && btn.DataContext is ProductoDto producto)
            {
                vm.EditarCommand.Execute(producto);
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductoViewModel vm && sender is Button btn && btn.DataContext is ProductoDto producto)
            {
                vm.DesactivarCommand.Execute(producto);
            }
        }
    }
}