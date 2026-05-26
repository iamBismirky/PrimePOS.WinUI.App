using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using PrimePOS.WinUI.Models;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.ViewModels;
using PrimePOS.WinUI.Views.Dialog;
using PrimePOS.WinUI.Views.Pages;
using System;
using System.Threading.Tasks;
using Windows.Graphics;
using WinRT.Interop;



namespace PrimePOS.WinUI;

public sealed partial class MainWindow : Window
{
    private readonly AppSesionViewModel _sesion;
    private readonly NotificationService _notify;
    private readonly OverlayService _overlayService;
    private readonly AuthOverlayService _authService;
    private readonly DashboardViewModel _dashboardViewModel;

    private bool _initialized;

    public MainWindow()
    {
        InitializeComponent();

        contentFrame.Navigate(typeof(DashboardPage));

        this.ExtendsContentIntoTitleBar = true;

        _sesion = App.Services.GetRequiredService<AppSesionViewModel>();
        _notify = App.Services.GetRequiredService<NotificationService>();
        _overlayService = App.Services.GetRequiredService<OverlayService>();
        _authService = App.Services.GetRequiredService<AuthOverlayService>();
        _dashboardViewModel = App.Services.GetRequiredService<DashboardViewModel>();


        RootGrid.DataContext = _sesion;

        SetWindowSizeAndCenter(1600, 900);

        this.Activated += MainWindow_Activated;


        _notify.OnNotification += notification =>
        {
            _ = MostrarNotificacionAsync(notification);
        };


    }


    private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (_initialized)
            return;

        _initialized = true;

        _overlayService.Initialize(OverlayHost);


        KeyboardService.Attach((UIElement)Content);


        await MostrarLoginAsync();
    }
    private async Task MostrarNotificacionAsync(
    AppNotification notification)
    {
        GlobalInfoBar.Title =
            notification.Title;

        GlobalInfoBar.Message =
            notification.Message;

        GlobalInfoBar.Severity =
            ConvertirSeverity(notification.Type);

        GlobalInfoBar.IsOpen = true;

        await Task.Delay(3000);

        GlobalInfoBar.IsOpen = false;
    }
    private static InfoBarSeverity ConvertirSeverity(
    AppNotificationType type)
    {
        return type switch
        {
            AppNotificationType.Success =>
                InfoBarSeverity.Success,

            AppNotificationType.Error =>
                InfoBarSeverity.Error,

            AppNotificationType.Warning =>
                InfoBarSeverity.Warning,

            _ =>
                InfoBarSeverity.Informational
        };
    }
    private async Task MostrarLoginAsync()
    {
        var loginVM =
            App.Services
                .GetRequiredService<LoginOverlayViewModel>();

        bool ok =
            await _overlayService
                .ShowLoginAsync(loginVM);

        if (ok)
        {
            await _dashboardViewModel
                .InicializarAsync();
        }
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
                        contentFrame.Navigate(typeof(DashboardPage));
                        _dashboardViewModel.Limpiar();
                        await MostrarLoginAsync();
                    }
                    sender.SelectedItem = null;
                    sender.SelectedItem = sender.MenuItems[0];
                    break;


            }
        }
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


    private void OverlayContainer_Tapped(object sender, TappedRoutedEventArgs e)
    {
        //_overlayService.Close();
    }



}
