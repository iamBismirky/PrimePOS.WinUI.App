using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using PrimePOS.DAL.Context;
using PrimePOS.ENTITIES.Models;
using PrimePOS.WinUI;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace PrimePOS.WinUI
{
    
    public partial class App : Application
    {
        private Window? _window;
        public static ElementTheme TemaActual = ElementTheme.Light;

        public App()
        {
            InitializeComponent();
            Servicios.Inicializar();
            RequestedTheme = ApplicationTheme.Light;
        }
        
       
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //_window = new LoginWindow();
            _window = new MainWindow();
            _window.Activate();
        }

    }
}
