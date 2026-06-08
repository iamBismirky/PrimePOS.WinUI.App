using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels.Pages;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Views.Pages
{

    public sealed partial class EmpresaPage : Page
    {
        private readonly EmpresaViewModel vm;
        public EmpresaPage()
        {
            InitializeComponent();
            vm = App.Services.GetRequiredService<EmpresaViewModel>();
            DataContext = vm;
        }
    }
}
