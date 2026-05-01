using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
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

    public ProductoViewModel(ProductoApiService apiProducto, CategoriaApiService apiCategoria, NotificationService notify)
    {
        _apiProducto = apiProducto;
        _apiCategoria = apiCategoria;
        _notify = notify;
    }



    [ObservableProperty] private ObservableCollection<ProductoDto> productos = new();
    [ObservableProperty] private ObservableCollection<CategoriaDto> categorias = new();
    [ObservableProperty] private ProductoDto? productoSeleccionado;
    [ObservableProperty] private CategoriaDto? categoriaSeleccionada;
    [ObservableProperty] private string codigo = "";
    [ObservableProperty] private string codigoBarra = "";
    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string descripcion = "";
    [ObservableProperty] private string categoriaNombre = "";
    [ObservableProperty] private decimal precioCompra;
    [ObservableProperty] private decimal precioVenta;
    [ObservableProperty] private int existencia;
    [ObservableProperty] private bool estado = true;
    [ObservableProperty] private DateTime fechaRegistro = DateTime.Today;
    [ObservableProperty] private string buscar = "";
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isOverlayVisible;
    [ObservableProperty] private bool esEdicion;
    private List<ProductoDto> _cache = new();

    public Visibility OverlayVisibility =>
        IsOverlayVisible ? Visibility.Visible : Visibility.Collapsed;

    partial void OnIsOverlayVisibleChanged(bool value)
    {
        OnPropertyChanged(nameof(OverlayVisibility));
    }

    public Visibility LoadingVisibility =>
        IsLoading ? Visibility.Visible : Visibility.Collapsed;

    partial void OnIsLoadingChanged(bool value)
    {
        OnPropertyChanged(nameof(LoadingVisibility));
    }
    public double PrecioCompraUI
    {
        get => (double)PrecioCompra;
        set => PrecioCompra = (decimal)value;
    }
    partial void OnPrecioCompraChanged(decimal value)
    {
        OnPropertyChanged(nameof(PrecioCompraUI));
    }
    public double PrecioVentaUI
    {
        get => (double)PrecioVenta;
        set => PrecioVenta = (decimal)value;
    }
    partial void OnPrecioVentaChanged(decimal value)
    {
        OnPropertyChanged(nameof(PrecioVentaUI));
    }
    public double ExistenciaUI
    {
        get => (double)Existencia;
        set => Existencia = (int)value;
    }
    partial void OnExistenciaChanged(int value)
    {
        OnPropertyChanged(nameof(ExistenciaUI));
    }

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
    public async Task GuardarAsync()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                _notify.Warning("El nombre es obligatorio");
                return;
            }
            if (CategoriaSeleccionada == null)
            {
                _notify.Warning("Debe seleccionar una categoria");
                return;
            }
            if (PrecioCompra <= 0)
            {
                _notify.Warning("El precio de compra debe ser mayor a cero");
                return;
            }
            if (PrecioVenta <= 0)
            {
                _notify.Warning("El precio de venta debe ser mayor a cero");
                return;
            }
            if (PrecioCompra <= PrecioVenta)
            {
                _notify.Warning("Precio de compra no puede ser igual o menor que precio venta");
            }
            if (Existencia < 0)
            {
                _notify.Warning("La existencia no puede ser negativa");
                return;
            }


            if (EsEdicion)
            {

                var dto = new ActualizarProductoDto
                {
                    ProductoId = ProductoSeleccionado!.ProductoId,
                    CodigoBarra = CodigoBarra.Trim(),
                    Nombre = Nombre.Trim(),
                    Descripcion = Descripcion.Trim(),
                    CategoriaId = CategoriaSeleccionada!.CategoriaId,
                    PrecioCompra = PrecioCompra,
                    PrecioVenta = PrecioVenta,
                    Existencia = Existencia,
                    Estado = Estado,
                };

                var res = await _apiProducto.ActualizarProductoAsync(ProductoSeleccionado.ProductoId, dto);
                if (!res.Success)
                {
                    _notify.Error(res.Message);
                    return;
                }

                _notify.Success(res.Message);

            }
            else
            {
                var dto = new CrearProductoDto
                {

                    CodigoBarra = CodigoBarra.Trim(),
                    Nombre = Nombre.Trim(),
                    Descripcion = Descripcion.Trim(),
                    CategoriaId = CategoriaSeleccionada!.CategoriaId,
                    PrecioCompra = PrecioCompra,
                    PrecioVenta = PrecioVenta,
                    Existencia = Existencia,
                    Estado = Estado,
                    FechaRegistro = DateTime.Now
                };

                var res = await _apiProducto.CrearProductoAsync(dto);

                if (!res.Success)
                {
                    _notify.Error(res.Message);
                    return;
                }

                _notify.Success(res.Message);

            }

            await CargarProductosAsync();
            Limpiar();
            CerrarOverlay();
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
    public async Task EditarAsync(ProductoDto producto)
    {
        EsEdicion = true;
        ProductoSeleccionado = producto;
        await CargarCategoriasAsync();

        Codigo = producto.Codigo;
        CodigoBarra = producto.CodigoBarra;
        Nombre = producto.Nombre;
        Descripcion = producto.Descripcion;
        CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.CategoriaId == producto.CategoriaId);
        PrecioCompra = producto.PrecioCompra;
        PrecioVenta = producto.PrecioVenta;
        Existencia = producto.Existencia;
        Estado = producto.Estado;
        FechaRegistro = producto.FechaRegistro;
        AbrirOverlay();
    }

    [RelayCommand]
    public async Task DesactivarAsync(ProductoDto producto)
    {
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
        try
        {
            IsLoading = true;
            Limpiar();
            await CargarCategoriasAsync();
            AbrirOverlay();
        }
        catch (Exception ex)
        {
            _notify.Error(ex.Message);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    public void Limpiar()
    {
        Codigo = "";
        CodigoBarra = "";
        Nombre = "";
        Descripcion = "";
        CategoriaNombre = "";
        PrecioCompra = 0;
        PrecioVenta = 0;
        Existencia = 0;
        FechaRegistro = DateTime.Now;
        Estado = true;
        ProductoSeleccionado = null;
        CategoriaSeleccionada = null;
        EsEdicion = false;
    }

    [RelayCommand]
    public void AbrirOverlay()
    {
        IsOverlayVisible = true;
    }

    [RelayCommand]
    public void CerrarOverlay()
    {
        IsOverlayVisible = false;
        Limpiar();
    }
}