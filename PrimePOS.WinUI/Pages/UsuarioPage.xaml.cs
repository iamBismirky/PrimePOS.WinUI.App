using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.WinUI.ViewModels;
using System;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class UsuarioPage : Page
    {
        public UsuarioViewModel ViewModel;
        public UsuarioPage()
        {
            InitializeComponent();

            ViewModel = App.AppServices.GetRequiredService<UsuarioViewModel>();
            DataContext = ViewModel;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarUsuariosCommand.ExecuteAsync(null);
        }

        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is UsuarioViewModel vm)
            {
                vm.FiltrarCommand.Execute(null);
            }

        }
        private async void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is UsuarioViewModel vm && sender is Button btn && btn.DataContext is UsuarioDto usuario)
            {
                vm.EditarCommand.Execute(usuario);
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is UsuarioViewModel vm && sender is Button btn && btn.DataContext is UsuarioDto usuario)
            {
                vm.DesactivarCommand.Execute(usuario);
            }
        }
    }
}
