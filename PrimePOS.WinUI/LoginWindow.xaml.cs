using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Drawing;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI
{
    
    public sealed partial class LoginWindow : Window
    {
        
        public LoginWindow()
        {
            InitializeComponent();
            // Quitar barra de título
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
        private async void BtnLogin_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                var loginDto = new AutenticarUsuarioDto
                {
                    Username = txtUsername.Text,
                    Password = pwdPassword.Password

                };
                
                var usuarioSesion = await Servicios.UsuarioService.AutenticarUsuarioAsync(loginDto);

                SesionUsuario.UsuarioId = usuarioSesion.UsuarioId;
                SesionUsuario.UsuarioNombre = usuarioSesion.UsuarioNombre;
                SesionUsuario.RolId = usuarioSesion.RolId;
                SesionUsuario.RolNombre = usuarioSesion.RolNombre;
                SesionUsuario.Activa = true;

                MainWindow main = new MainWindow();
                main.Activate();
                this.Close();
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "PrimePOS",
                    Content = ex.Message,
                    CloseButtonText = "Aceptar",
                    XamlRoot = this.Content.XamlRoot
                };

                await dialog.ShowAsync();
            }

        }
        private async void BtnSalir_Click(object sender, RoutedEventArgs e) { }
        private void BtnTema_Click(object sender, RoutedEventArgs e)
        {
            if (App.TemaActual == ElementTheme.Light)
            {
                RootGrid.RequestedTheme = ElementTheme.Dark;
                iconTema.Glyph = "\uE708"; // luna
                App.TemaActual = ElementTheme.Dark;
            }
            else
            {
                RootGrid.RequestedTheme = ElementTheme.Light;
                iconTema.Glyph = "\uE706"; // sol
                App.TemaActual = ElementTheme.Light;
            }
        }
    }
}
