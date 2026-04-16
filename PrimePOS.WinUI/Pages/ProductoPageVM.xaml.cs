using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PrimePOS.BLL.Services;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Overlays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductoPageVM : Page
    {
        private readonly ProductoService _productoService;
        private List<ProductoDto> _cacheProductos = new();
        public ProductoPageVM()
        {
            InitializeComponent();
            _productoService = App.Services.GetRequiredService<ProductoService>();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await CargarProductosAsync();
        }
        private async Task CargarProductosAsync()
        {
            var lista = await _productoService.ObtenerTodosAsync();
            _cacheProductos = lista.ToList();
            dgProductos.ItemsSource = _cacheProductos;
        }

        // 🔍 BUSCADOR
        private void TxtBuscar_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
            {
                dgProductos.ItemsSource = _cacheProductos;
                sender.ItemsSource = null;
                return;
            }

            var filtrados = _cacheProductos
                .Where(x => x.Nombre.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                .ToList();

            sender.ItemsSource = filtrados.Take(5).Select(x => x.Nombre);
        }

        private void TxtBuscar_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Filtrar(sender.Text);
        }

        private void TxtBuscar_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
            if (sender.Text != null)
            {
                Filtrar(sender.Text);

            }
        }

        private void Filtrar(string texto)
        {
            dgProductos.ItemsSource = _cacheProductos
                .Where(x => x.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {

            if ((sender as Button)?.DataContext is ProductoDto producto)
            {
                var overlay = new ProductoOverlay();

                await overlay.SetData(producto);
                overlay.OnClose += CerrarOverlay;
                //overlay.OnCrear += GuardarProducto;
                overlay.OnActualizar += ActualizarProducto;

                OverlayContent.Content = overlay;
                OverlayContainer.Visibility = Visibility.Visible;
            }
        }
        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {

            if ((sender as Button)?.DataContext is ProductoDto producto)
            {
                await _productoService.EliminarProductoAsync(producto.ProductoId);


                await CargarProductosAsync();
            }
        }
        private async void GuardarProducto(CrearProductoDto dto)
        {
            await _productoService.CrearProductoAsync(dto);
            await CargarProductosAsync();
        }
        private async void ActualizarProducto(ActualizarProductoDto dto)
        {
            await _productoService.ActualizarProductoAsync(dto);
            await CargarProductosAsync();
        }


        private void CerrarOverlay()
        {
            OverlayContent.Content = null;
            OverlayContainer.Visibility = Visibility.Collapsed;
        }
        private void AutoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }

        private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {


        }
        private void MostrarOverlay(UserControl overlay)
        {
            // cerrar desde el overlay
            if (overlay is ProductoOverlay po)
            {
                po.OnClose += CerrarOverlay;
            }

            OverlayContent.Content = overlay;
            OverlayContainer.Visibility = Visibility.Visible;
        }
        private async void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            var overlay = new ProductoOverlay();
            overlay.OnCrear += GuardarProducto;
            MostrarOverlay(overlay);

        }
    }
}
