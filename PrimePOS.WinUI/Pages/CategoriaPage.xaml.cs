using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.WinUI.ViewModels;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class CategoriaPage : Page
    {
        private readonly CategoriaViewModel ViewModel;
        public CategoriaPage()
        {
            InitializeComponent();
            ViewModel = App.AppServices.GetRequiredService<CategoriaViewModel>();
            DataContext = ViewModel;

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarCommand.ExecuteAsync(null);
        }

        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is CategoriaViewModel vm)
            {
                vm.FiltrarCommand.Execute(null);
            }

        }
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CategoriaViewModel vm && sender is Button btn && btn.DataContext is CategoriaDto categoria)
            {
                vm.EditarCommand.Execute(categoria);
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CategoriaViewModel vm && sender is Button btn && btn.DataContext is CategoriaDto categoria)
            {
                vm.DesactivarCommand.Execute(categoria);
            }
        }
    }
}