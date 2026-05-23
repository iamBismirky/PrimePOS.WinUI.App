using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.ViewModels.Overlays;
using PrimePOS.WinUI.Views.Overlays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class ProductoViewModel : ObservableObject
{
    private readonly ProductoApiService _apiProducto;
    private readonly CategoriaApiService _apiCategoria;
    private readonly NotificationService _notify;
    private readonly OverlayService _overlayService;
    private readonly PdfViewService _pdfService;

    public ProductoViewModel(ProductoApiService apiProducto,
        CategoriaApiService apiCategoria,
        NotificationService notify,
        OverlayService overlayService,
        PdfViewService pdfService)
    {
        _apiProducto = apiProducto;
        _apiCategoria = apiCategoria;
        _notify = notify;
        _overlayService = overlayService;
        _pdfService = pdfService;
    }



    [ObservableProperty] private ObservableCollection<ProductoDto> productos = new();
    [ObservableProperty] private ObservableCollection<CategoriaDto> categorias = new();
    [ObservableProperty] private ProductoDto? productoSeleccionado;
    [ObservableProperty] private CategoriaDto? categoriaSeleccionada;
    [ObservableProperty] private string buscar = "";
    [ObservableProperty] private bool isLoading;
    private List<ProductoDto> _cache = new();


    [RelayCommand]
    public async Task CargarProductosAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _apiProducto.ObtenerProductosAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message ?? "Error al cargar productos");
                return;
            }

            _cache = res.Data ?? new List<ProductoDto>();
            Productos = new ObservableCollection<ProductoDto>(res.Data ?? new());
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
    [RelayCommand]
    public async Task CargarCategoriasAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _apiCategoria.ObtenerCategoriasAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message);
                return;
            }


            Categorias = new ObservableCollection<CategoriaDto>(res.Data ?? new());
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }


    [RelayCommand]
    public async Task EditarAsync(ProductoDto productoSeleccionado)
    {
        if (productoSeleccionado is null)
            return;

        var vm = App.Services
            .GetRequiredService<ProductoOverlayViewModel>();

        await vm.InicializarAsync(productoSeleccionado);

        var overlay = new ProductoOverlay(vm);

        var actualizado =
            await _overlayService.ShowAsync(overlay, vm);

        if (!actualizado)
            return;

        await CargarProductosAsync();
    }

    [RelayCommand]
    public async Task DesactivarAsync(ProductoDto producto)
    {
        var confirmar = await _overlayService.ConfirmAsync("Confirmar desactivación",
            $"¿Está seguro de desactivar {producto.Nombre}?");

        if (!confirmar)
            return;

        var res = await _apiProducto.DesactivarProductoAsync(producto.ProductoId);

        if (!res.Success)
        {
            _notify.Error(res.Message);
            return;
        }

        _notify.Success(res.Message);
        await CargarProductosAsync();
    }

    [RelayCommand]
    public void Filtrar()
    {
        if (string.IsNullOrWhiteSpace(Buscar))
        {
            Productos = new ObservableCollection<ProductoDto>(_cache);
            return;
        }

        var filtradas = _cache
            .Where(x => (x.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase)) ||
            (x.Codigo?.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (x.CodigoBarra?.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (x.CategoriaNombre?.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ?? false)

            ).ToList();

        Productos = new ObservableCollection<ProductoDto>(filtradas);
    }

    [RelayCommand]
    public async Task NuevoAsync()
    {
        var vm = App.Services.GetRequiredService<ProductoOverlayViewModel>();
        await vm.InicializarAsync(null);
        var overlay = new ProductoOverlay(vm);
        var creado = await _overlayService.ShowAsync(overlay, vm);
        if (!creado)
            return;
        if (creado)
        {
            await CargarProductosAsync();
        }

    }


    [RelayCommand]
    public async Task VerEtiquetaAsync(ProductoDto producto)
    {
        ProductoSeleccionado = producto;
        var pdf = await _apiProducto.ObtenerEtiquetaAsync(producto.ProductoId);
        await _pdfService.MostrarEtiquetaPdfAsync(pdf);
    }
}