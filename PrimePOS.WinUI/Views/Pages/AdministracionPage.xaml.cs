using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.WinUI.ViewModels.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdministracionPage : Page
    {
        private readonly AdministracionViewModel vm;
        public AdministracionPage()
        {
            InitializeComponent();
            vm = App.Services.GetRequiredService<AdministracionViewModel>();
            DataContext = vm;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (DataContext is AdministracionViewModel vm)
            {
                await vm.CargarEmpresasAsync();
            }
        }
        private void Desactivar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Configurar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
