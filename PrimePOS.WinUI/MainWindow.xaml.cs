using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.Infrastructure;
using PrimePOS.WinUI.Pages;
using PrimePOS.WinUI.ViewModels;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly AppSesionViewModel _sesion;
        public string UsuarioNombre => Sesion.UsuarioNombre;
        public string RolNombre => Sesion.RolNombre;
        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Navigate(typeof(DashboardPage));
            RootGrid.RequestedTheme = App.TemaActual;
            this.ExtendsContentIntoTitleBar = true;

            _sesion = App.Services.GetRequiredService<AppSesionViewModel>();
            RootGrid.DataContext = _sesion;


        }
        private void TitleBar_BackRequested(TitleBar sender, object args)
        {
            if (contentFrame.CanGoBack)
            {
                contentFrame.GoBack();
            }

        }
        private void BtnTema_Click(object sender, RoutedEventArgs e)
        {
            if (App.TemaActual == ElementTheme.Light)
            {
                RootGrid.RequestedTheme = ElementTheme.Dark;
                iconTema.Glyph = "\uE706"; // luna
                App.TemaActual = ElementTheme.Dark;
            }
            else
            {
                RootGrid.RequestedTheme = ElementTheme.Light;
                iconTema.Glyph = "\uE708"; // sol
                App.TemaActual = ElementTheme.Light;
            }
        }
        private void SetThemeForWindow(Window window, ElementTheme theme)
        {
            if (window.Content is FrameworkElement root)
            {
                root.RequestedTheme = theme;
            }
        }
        private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
        {
            navView.IsPaneOpen = !navView.IsPaneOpen;
        }
        private async void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            if (args.SelectedItemContainer is NavigationViewItem item
                && contentFrame != null)
            {
                switch (item.Tag?.ToString())
                {
                    case "dashboard":
                        contentFrame?.Navigate(typeof(DashboardPage));
                        break;

                    case "ventas":
                        contentFrame?.Navigate(typeof(VentasPage));
                        break;
                    case "inventario":
                        contentFrame?.Navigate(typeof(InventarioPage));
                        break;
                    case "reportes":
                        contentFrame?.Navigate(typeof(ReportesPage));
                        break;
                    case "administrar":
                        contentFrame?.Navigate(typeof(AdministrarPage));
                        break;
                    case "perfil":
                        contentFrame?.Navigate(typeof(PerfilPage));
                        break;
                    case "cerrarSesion":
                        var dialog = new CerrarSesionDialog
                        {
                            XamlRoot = this.Content.XamlRoot,
                            RequestedTheme = App.TemaActual
                        };
                        var result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            _sesion.CerrarSesion();
                            App.IrALogin();
                        }
                        sender.SelectedItem = null;
                        break;


                }
            }
        }


    }
}
