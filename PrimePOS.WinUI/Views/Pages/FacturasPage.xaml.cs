using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.WinUI.ViewModels.Pages;
using System;


namespace PrimePOS.WinUI.Views.Pages
{
    public sealed partial class FacturasPage : Page
    {
        private readonly FacturaViewModel vm;
        public FacturasPage()
        {
            InitializeComponent();
            vm = App.Services.GetRequiredService<FacturaViewModel>();
            DataContext = vm;
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await vm.CargarFacturasAsync();
        }
    }
}
