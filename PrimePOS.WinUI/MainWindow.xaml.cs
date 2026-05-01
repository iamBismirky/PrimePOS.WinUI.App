using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.Pages;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Graphics;
using WinRT.Interop;



namespace PrimePOS.WinUI;

public sealed partial class MainWindow : Window
{
    public AppSesionViewModel _sesion;
    public NotificationService _notify;
    public MainWindow()
    {
        InitializeComponent();
        contentFrame.Navigate(typeof(DashboardPage));
        RootGrid.RequestedTheme = App.TemaActual;
        this.ExtendsContentIntoTitleBar = true;

        _sesion = App.AppServices.GetRequiredService<AppSesionViewModel>();
        _notify = App.AppServices.GetRequiredService<NotificationService>();
        RootGrid.DataContext = _sesion;
        SetWindowSizeAndCenter(1600, 900);


        _notify.OnNotify += (msg, type) =>
        {
            _ = MostrarNotificacionAsync(msg, type);
        };


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
    private async void navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {

        if (args.InvokedItemContainer is not NavigationViewItem item)
            return;
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
                        var login = new LoginWindow();
                        login.Activate();
                        this.Close();
                    }
                    sender.SelectedItem = null;
                    sender.SelectedItem = sender.MenuItems[0];
                    break;


            }
        }
    }
    private async Task MostrarNotificacionAsync(string msg, InfoBarSeverity type)
    {
        GlobalInfoBar.Message = msg;
        GlobalInfoBar.Severity = type;
        GlobalInfoBar.IsOpen = true;

        await Task.Delay(3000);

        GlobalInfoBar.IsOpen = false;
    }
    private void SetWindowSizeAndCenter(int width, int height)
    {
        var hwnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        // 🔥 Tamaño
        appWindow.Resize(new SizeInt32(width, height));

        // 🔥 Obtener tamaño de pantalla
        var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
        var workArea = displayArea.WorkArea;

        // 🔥 Calcular centro
        int x = workArea.X + (workArea.Width - width) / 2;
        int y = workArea.Y + (workArea.Height - height) / 2;

        // 🔥 Mover ventana
        appWindow.Move(new PointInt32(x, y));
    }


}
