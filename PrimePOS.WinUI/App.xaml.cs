using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using PrimePOS.WinUI.Config;
using QuestPDF.Infrastructure;
using System;


namespace PrimePOS.WinUI
{

    public partial class App : Application
    {

        public static Window? _window { get; private set; }

        public static ElementTheme TemaActual = ElementTheme.Dark;
        public static IServiceProvider AppServices { get; private set; } = null!;


        public App()
        {
            InitializeComponent();
            RequestedTheme = ApplicationTheme.Dark;

            // Configurar DI
            var serviceCollection = new ServiceCollection();

            //  Aquí inyectas TODO
            serviceCollection.AddApplicationServices();

            AppServices = serviceCollection.BuildServiceProvider();

            QuestPDF.Settings.License = LicenseType.Community;



        }


        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new LoginWindow();
            //_window = App.AppServices.GetRequiredService<MainWindow>();
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
