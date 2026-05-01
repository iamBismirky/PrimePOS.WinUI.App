using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.ViewModels;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class RolPage : Page
    {
        private readonly RolViewModel ViewModel;
        public RolPage()
        {
            InitializeComponent();
            ViewModel = App.AppServices.GetRequiredService<RolViewModel>();
            DataContext = ViewModel;

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarCommand.ExecuteAsync(null);
        }

        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is RolViewModel vm)
            {
                vm.FiltrarCommand.Execute(null);
            }

        }
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is RolViewModel vm && sender is Button btn && btn.DataContext is RolDto rol)
            {
                vm.EditarCommand.Execute(rol);
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is RolViewModel vm && sender is Button btn && btn.DataContext is RolDto rol)
            {
                vm.DesactivarCommand.Execute(rol);
            }
        }
    }
}