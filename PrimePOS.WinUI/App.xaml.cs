using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using PrimePOS.WinUI.Config;
using PrimePOS.WinUI.Infrastructure.PrimePOS.WinUI.Infrastructure;
using QuestPDF.Infrastructure;
using System;


namespace PrimePOS.WinUI
{

    public partial class App : Application
    {

        public static Window? MainAppWindow { get; private set; }

        public static ElementTheme TemaActual = ElementTheme.Dark;
        public static IServiceProvider Services { get; private set; } = null!;
        public static IServiceProvider AppServices { get; private set; } = null!;


        public App()
        {
            InitializeComponent();
            RequestedTheme = ApplicationTheme.Dark;

            // Configurar DI
            var services = new ServiceCollection();
            var serviceCollection = new ServiceCollection();

            //  Aquí inyectas TODO
            serviceCollection.AddApplicationServices();

            AppServices = serviceCollection.BuildServiceProvider();

            QuestPDF.Settings.License = LicenseType.Community;

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=PrimePOS_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            services.AddPrimePOSServices(connectionString);

            //Services = services.BuildServiceProvider();
        }


        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //_window = new LoginWindow();
            MainAppWindow = App.AppServices.GetRequiredService<MainWindow>();
            MainAppWindow.Activate();
        }
        public static void IrALogin()
        {

            var loginWindow = new LoginWindow();
            loginWindow.Activate();


            if (MainAppWindow != null)
            {
                MainAppWindow.Close();
            }
        }

    }
}
