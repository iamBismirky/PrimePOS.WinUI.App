using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI
{

    public sealed partial class LoginWindow : Window
    {
        public LoginViewModel ViewModel { get; }
        public LoginWindow()
        {
            InitializeComponent();

            ViewModel = App.AppServices.GetRequiredService<LoginViewModel>();
            RootGrid.DataContext = ViewModel;

            ViewModel.LoginSuccess += OnLoginExitoso;
            ViewModel.ErrorOcurrido += MostrarError;

            txtUsername.Focus(FocusState.Programmatic);
            this.ExtendsContentIntoTitleBar = true;
            this.SystemBackdrop = new MicaBackdrop();
            var hwnd = WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            var presenter = appWindow.Presenter as OverlappedPresenter;

            if (presenter != null)
            {
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
                presenter.IsResizable = false;

            }

            // Tamaño ventana
            appWindow.Resize(new Windows.Graphics.SizeInt32(420, 700));

            // Centrar ventana
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            var area = displayArea.WorkArea;

            appWindow.Move(new Windows.Graphics.PointInt32(
                (area.Width - 420) / 2,
                (area.Height - 700) / 2));

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
            //Abrir MainWindow
            //var main = new MainWindow();
            //main.Activate();

            //this.Close();
        }
        private void MostrarError(string mensaje)
        {
            infoError.Message = mensaje;
            infoError.IsOpen = true;

            AutoCloseInfoBar();
        }
        private async void AutoCloseInfoBar()
        {
            await Task.Delay(3000);
            infoError.IsOpen = false;
        }
    }

}
