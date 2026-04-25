using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using PrimePOS.WinUI.Services;
using System;
using System.Threading.Tasks;
using WinRT.Interop;


namespace PrimePOS.WinUI;


public sealed partial class LoginWindow : Window
{
    public LoginViewModel _viewModel;
    public NotificationService _notify;
    public LoginWindow()
    {
        InitializeComponent();
        ConfigurarVentana();
        ConfigurarUI();

        _viewModel = App.AppServices.GetRequiredService<LoginViewModel>();
        _notify = App.AppServices.GetRequiredService<NotificationService>();

        RootGrid.DataContext = _viewModel;
        _viewModel.LoginSuccess += OnLoginExitoso;




        _notify.OnNotify += (msg, type) =>
        {
            _ = MostrarNotificacionAsync(msg, type);
        };

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

    private void pwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (RootGrid.DataContext is LoginViewModel vm)
        {
            vm.Password = pwdPassword.Password;
        }

    }
    private void OnLoginExitoso()
    {

        var main = new MainWindow();
        main.Activate();

        this.Close();
    }
    private async Task MostrarNotificacionAsync(string msg, InfoBarSeverity type)
    {
        GlobalInfoBar.Message = msg;
        GlobalInfoBar.Severity = type;
        GlobalInfoBar.IsOpen = true;

        await Task.Delay(3000);

        GlobalInfoBar.IsOpen = false;
    }
    private void ConfigurarVentana()
    {
        var hwnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.IsResizable = false;
        }

        const int width = 420;
        const int height = 700;

        appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));

        var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary).WorkArea;

        var x = displayArea.X + (displayArea.Width - width) / 2;
        var y = displayArea.Y + (displayArea.Height - height) / 2;

        appWindow.Move(new Windows.Graphics.PointInt32(x, y));
    }
    private void ConfigurarUI()
    {
        txtUsername.Focus(FocusState.Programmatic);

        this.ExtendsContentIntoTitleBar = true;
        this.SystemBackdrop = new MicaBackdrop();
    }
}
