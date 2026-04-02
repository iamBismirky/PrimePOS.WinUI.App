using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using PrimePOS.WinUI.Infrastructure.PrimePOS.WinUI.Infrastructure;
using QuestPDF.Infrastructure;
using System;


namespace PrimePOS.WinUI
{

    public partial class App : Application
    {
        private static Window? _window;
        public static ElementTheme TemaActual = ElementTheme.Dark;
        public static IServiceProvider Services { get; private set; } = null!;

        public App()
        {
            InitializeComponent();
            RequestedTheme = ApplicationTheme.Dark;

            // Configurar DI
            var services = new ServiceCollection();

            QuestPDF.Settings.License = LicenseType.Community;

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=PrimePOS_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            services.AddPrimePOSServices(connectionString);

            Services = services.BuildServiceProvider();
        }


        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new LoginWindow();
            //_window = new MainWindow();
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
