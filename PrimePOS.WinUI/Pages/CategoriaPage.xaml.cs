using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Categoria;
using PrimePOS.WinUI.Infrastructure;
using System;

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

        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategorias.SelectedItem is CategoriaDto categoria)
            {
                _categoriaIdSeleccionado = categoria.CategoriaId;
                txtNombre.Text = categoria.Nombre;
                tgEstado.IsOn = categoria.Estado;
            }
        }
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

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);


            }

        }
        private async void BtnActualizarCategoria_Click(object sender, RoutedEventArgs e)
        {
            try
            {

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

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
        }
        private async void BtnEliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var dto = new CategoriaDto
                {
                    CategoriaId = _categoriaIdSeleccionado

                };

                await Servicios.CategoriaService.EliminarCategoriaAsync(dto);
                ListarCategorias();
                LimpiarCampos();


            }
            catch (Exception ex)
            {
                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
        }
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private async void ListarCategorias()
        {
            try
            {
                var categorias = await Servicios.CategoriaService.ListarCategoriasAsync();
                dgCategorias.ItemsSource = categorias;
            }
            catch (Exception ex)
            {

                await DialogHelper.MostrarMensaje(this.XamlRoot, "Error", ex.Message);

            }
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            _categoriaIdSeleccionado = 0;

        }

    }
}
