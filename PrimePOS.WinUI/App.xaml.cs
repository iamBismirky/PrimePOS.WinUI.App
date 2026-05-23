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
        public static IServiceProvider Services { get; private set; } = null!;
        public static ElementTheme TemaActual { get; internal set; }

        public App()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddApplicationServices();

            Services = serviceCollection.BuildServiceProvider();

            QuestPDF.Settings.License = LicenseType.Community;

        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = App.Services.GetRequiredService<MainWindow>();
            _window.Activate();
        }


    }
}
