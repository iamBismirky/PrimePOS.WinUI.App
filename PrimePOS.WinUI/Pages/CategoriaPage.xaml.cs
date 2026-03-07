using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.DTOs.Categoria;
using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.WinUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoriaPage : Page
    {
        private int _categoriaIdSeleccionado = 0;
        public CategoriaPage()
        {
            InitializeComponent();
            ListarCategorias();
        }
        
        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private async void BtnCrearCategoria_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                var dto = new CategoriaDto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Estado = tgEstado.IsOn
                };

                await Servicios.CategoriaService.CrearCategoriaAsync(dto);
                ListarCategorias();
                LimpiarCampos();
            }
            catch (Exception ex)
            {

                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Aceptar",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }

        }
        private async void BtnActualizarCategoria_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_categoriaIdSeleccionado == 0)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Debe de seleccionar un rol para editar",
                        CloseButtonText = "Aceptar",
                        XamlRoot = this.XamlRoot
                    };
                    return;

                }

                var dto = new CategoriaDto
                {
                    CategoriaId = _categoriaIdSeleccionado,
                    Nombre = txtNombre.Text,
                    Estado = (bool)tgEstado.IsOn

                };

                await Servicios.CategoriaService.ActualizarCategoriaAsync(dto);
                ListarCategorias();
                LimpiarCampos();


            }
            catch (Exception ex)
            {

                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Aceptar",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
        private async void BtnEliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_categoriaIdSeleccionado == 0)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Debe de seleccionar un rol para eliminar",
                        CloseButtonText = "Aceptar",
                        XamlRoot = this.XamlRoot
                    };
                    return;

                }

                var dto = new EliminarRolDto
                {
                    RolId = _categoriaIdSeleccionado

                };

                await Servicios.RolService.EliminarRolAsync(dto);
                ListarCategorias();
                LimpiarCampos();


            }
            catch (Exception ex)
            {

                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Aceptar",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e) 
        {
            LimpiarCampos();
        }

        private async void ListarCategorias()
        {
            var categorias = await Servicios.CategoriaService.ListarCategoriasAsync();
            dgCategorias.ItemsSource = categorias;
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            
        }

    }
}
