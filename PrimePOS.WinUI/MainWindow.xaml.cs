using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.WinUI.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Navigate(typeof(DashboardPage));
            //navView.SelectionChanged += navView_SelectionChanged;
        }
        private void navView_SelectionChanged(NavigationView sender,
                                       NavigationViewSelectionChangedEventArgs args)
        {

            if (args.SelectedItemContainer is NavigationViewItem item
        && contentFrame != null) { 
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
                    
                    
                }
            }
        }

        
    }
}
