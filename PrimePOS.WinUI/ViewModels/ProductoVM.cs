//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using Microsoft.UI.Xaml.Controls;
//using PrimePOS.BLL.DTOs.Producto;
//using PrimePOS.BLL.Services;
//using PrimePOS.Contracts.DTOs.Categoria;
//using PrimePOS.WinUI.Overlays;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;

//public partial class ProductoVM : ObservableObject
//{
//    private readonly ProductoService _productoService;
//    private readonly CategoriaService _categoriaService;

//    // 📋 Lista principal (grid)
//    public ObservableCollection<ProductoDto> Productos { get; set; } = new();

//    // 🔍 Sugerencias (AutoSuggest)
//    public ObservableCollection<ProductoDto> Sugerencias { get; set; } = new();

//    public ObservableCollection<CategoriaDto> Categorias { get; set; } = new();

//    private List<ProductoDto> _cacheProductos = new();

//    [ObservableProperty]
//    private ProductoDto? productoSeleccionado;

//    [ObservableProperty]
//    private bool mostrarOverlay;

//    [ObservableProperty]
//    private string busqueda;

//    [ObservableProperty]
//    private bool isLoading;

//    public event Action<UserControl> OnMostrarOverlay;

//    public ProductoVM(ProductoService productoService, CategoriaService categoriaService)
//    {
//        _productoService = productoService;
//        _categoriaService = categoriaService;
//        _ = CargarDatosAsync();
//    }

//    // =========================
//    // 📥 CARGAR TODO
//    // =========================
//    [RelayCommand]
//    public async Task CargarDatosAsync()
//    {
//        IsLoading = true;

//        var productos = await _productoService.ListarProductosAsync();
//        _cacheProductos = productos.ToList();

//        Productos.Clear();
//        foreach (var p in productos)
//            Productos.Add(p);

//        var categorias = await _categoriaService.ListarCategoriasAsync();
//        Categorias.Clear();
//        foreach (var c in categorias)
//            Categorias.Add(c);

//        IsLoading = false;
//    }

//    // =========================
//    // 🔍 BUSCAR (AutoSuggest + Grid)
//    // =========================
//    partial void OnBusquedaChanged(string value)
//    {
//        if (string.IsNullOrWhiteSpace(value))
//        {
//            RestaurarLista();
//            Sugerencias.Clear();
//            return;
//        }

//        var filtrados = _cacheProductos
//            .Where(p => p.Nombre.Contains(value, StringComparison.OrdinalIgnoreCase))
//            .ToList();

//        // 🔍 sugerencias
//        Sugerencias.Clear();
//        foreach (var item in filtrados.Take(10))
//            Sugerencias.Add(item);

//        // 📋 grid filtrado
//        Productos.Clear();
//        foreach (var item in filtrados)
//            Productos.Add(item);
//    }

//    private void RestaurarLista()
//    {
//        Productos.Clear();
//        foreach (var item in _cacheProductos)
//            Productos.Add(item);
//    }

//    // =========================
//    // 🎯 Seleccionar desde buscador
//    // =========================
//    [RelayCommand]
//    private void SeleccionarDesdeBusqueda(ProductoDto producto)
//    {
//        if (producto == null) return;

//        Editar(producto);
//    }

//    // =========================
//    // 🆕 NUEVO
//    // =========================
//    [RelayCommand]
//    private void Nuevo()
//    {
//        ProductoSeleccionado = new ProductoDto
//        {
//            Estado = true
//        };

//        var overlay = new ProductoOverlay
//        {
//            DataContext = this
//        };

//        OnMostrarOverlay?.Invoke(overlay);
//    }

//    // =========================
//    // ✏️ EDITAR
//    // =========================
//    [RelayCommand]
//    private void Editar(ProductoDto producto)
//    {
//        if (producto == null) return;
//        System.Diagnostics.Debug.WriteLine(producto.Nombre);
//        ProductoSeleccionado = new ProductoDto
//        {
//            ProductoId = producto.ProductoId,
//            Nombre = producto.Nombre,
//            Descripcion = producto.Descripcion,
//            CodigoBarra = producto.CodigoBarra,
//            CategoriaId = producto.CategoriaId,
//            PrecioCompra = producto.PrecioCompra,
//            PrecioVenta = producto.PrecioVenta,
//            Existencia = producto.Existencia,
//            Estado = producto.Estado
//        };

//        ProductoSeleccionado = new ProductoDto
//        {
//            Estado = true
//        };

//        var overlay = new ProductoOverlay
//        {
//            DataContext = this
//        };

//        OnMostrarOverlay?.Invoke(overlay);
//    }

//    // =========================
//    // 💾 GUARDAR
//    // =========================
//    [RelayCommand]
//    private async Task GuardarAsync()
//    {
//        if (ProductoSeleccionado == null) return;


//        if (ProductoSeleccionado.ProductoId == 0)
//        {
//            await _productoService.CrearProductoAsync(new CrearProductoDto
//            {
//                Nombre = ProductoSeleccionado.Nombre,
//                Descripcion = ProductoSeleccionado.Descripcion,
//                CodigoBarra = ProductoSeleccionado.CodigoBarra,
//                CategoriaId = ProductoSeleccionado.CategoriaId,
//                PrecioCompra = ProductoSeleccionado.PrecioCompra,
//                PrecioVenta = ProductoSeleccionado.PrecioVenta,
//                Existencia = ProductoSeleccionado.Existencia,
//                Estado = ProductoSeleccionado.Estado
//            });
//        }
//        else
//        {
//            await _productoService.ActualizarProductoAsync(new ActualizarProductoDto
//            {
//                ProductoId = ProductoSeleccionado.ProductoId,
//                Nombre = ProductoSeleccionado.Nombre,
//                Descripcion = ProductoSeleccionado.Descripcion,
//                CodigoBarra = ProductoSeleccionado.CodigoBarra,
//                CategoriaId = ProductoSeleccionado.CategoriaId,
//                PrecioCompra = ProductoSeleccionado.PrecioCompra,
//                PrecioVenta = ProductoSeleccionado.PrecioVenta,
//                Existencia = ProductoSeleccionado.Existencia,
//                Estado = ProductoSeleccionado.Estado
//            });
//        }

//        MostrarOverlay = false;
//        await CargarDatosAsync();
//    }

//    // =========================
//    // ❌ ELIMINAR
//    // =========================
//    [RelayCommand]
//    private async Task EliminarAsync(ProductoDto producto)
//    {
//        if (producto == null) return;

//        await _productoService.EliminarProductoAsync(new EliminarProductoDto
//        {
//            ProductoId = producto.ProductoId
//        });

//        await CargarDatosAsync();
//    }

//    // =========================
//    // 🚪 CERRAR
//    // =========================
//    [RelayCommand]
//    private void CerrarOverlay()
//    {
//        MostrarOverlay = false;
//    }
//}