using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Cliente;
using PrimePOS.WinUI.ViewModels;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class ClientePage : Page
    {
        private readonly ClienteViewModel ViewModel;
        public ClientePage()
        {
            InitializeComponent();
            ViewModel = App.AppServices.GetRequiredService<ClienteViewModel>();

            this.DataContext = ViewModel;

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarClientesCommand.ExecuteAsync(null);
        }

        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ClienteViewModel vm)
            {
                vm.FiltrarCommand.Execute(null);
            }

        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ClienteViewModel vm && sender is Button btn && btn.DataContext is ClienteDto cliente)
            {
                vm.EditarCommand.Execute(cliente);
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ClienteViewModel vm && sender is Button btn && btn.DataContext is ClienteDto cliente)
            {
                vm.DesactivarCommand.Execute(cliente);
            }
        }
    }
}