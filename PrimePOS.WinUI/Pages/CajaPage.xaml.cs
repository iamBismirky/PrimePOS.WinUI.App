using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Caja;
using PrimePOS.WinUI.ViewModels;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class CajaPage : Page
    {
        private readonly CajaViewModel ViewModel;
        public CajaPage()
        {
            InitializeComponent();
            ViewModel = App.AppServices.GetRequiredService<CajaViewModel>();

            this.DataContext = ViewModel;

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarCommand.ExecuteAsync(null);
        }

        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is CajaViewModel vm)
            {
                vm.FiltrarCommand.Execute(null);
            }

        }
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CajaViewModel vm && sender is Button btn && btn.DataContext is CajaDto caja)
            {
                vm.EditarCommand.Execute(caja);
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CajaViewModel vm && sender is Button btn && btn.DataContext is CajaDto caja)
            {
                vm.DesactivarCommand.Execute(caja);
            }
        }
    }
}