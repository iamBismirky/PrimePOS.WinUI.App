using Microsoft.UI.Xaml;
using PrimePOS.WinUI.Infrastructure;


namespace PrimePOS.WinUI
{

    public partial class App : Application
    {
        private static Window? _window;
        public static ElementTheme TemaActual = ElementTheme.Dark;

        public App()
        {
            InitializeComponent();
            Servicios.Inicializar();
            RequestedTheme = ApplicationTheme.Dark;
        }


        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //_window = new LoginWindow();
            _window = new MainWindow();
            _window.Activate();
        }
        public static void IrALogin()
        {

            var loginWindow = new LoginWindow();
            loginWindow.Activate();


            if (_window != null)
            {
                _window.Close();
            }
        }

    }
}
