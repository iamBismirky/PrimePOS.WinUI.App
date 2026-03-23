using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.BLL.DTOs.Categoria;
using PrimePOS.BLL.Services;
using PrimePOS.WinUI.Helpers;
using System;
using System.Threading.Tasks;


namespace PrimePOS.WinUI.Pages
{

    public sealed partial class CategoriaPage : Page
    {
        private readonly CategoriaService _categoriaService;
        private int _categoriaIdSeleccionado = 0;
        public CategoriaPage()
        {
            InitializeComponent();

            _categoriaService = App.Services.GetRequiredService<CategoriaService>();

        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ListarCategoriasAsync();
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

                await _categoriaService.CrearCategoriaAsync(dto);
                await ListarCategoriasAsync();
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

                await _categoriaService.ActualizarCategoriaAsync(dto);
                await ListarCategoriasAsync();
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

                await _categoriaService.EliminarCategoriaAsync(dto);
                await ListarCategoriasAsync();
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

        private async Task ListarCategoriasAsync()
        {
            try
            {
                var categorias = await _categoriaService.ListarCategoriasAsync();
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
