using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class ProductoOverlayViewModel : ObservableObject
{
    private readonly ProductoApiService _apiProducto;
    private readonly CategoriaApiService _apiCategoria;
    private readonly NotificationService _notify;

    private readonly TaskCompletionSource<bool> _tcs = new();

    public Task<bool> WaitTask => _tcs.Task;

    public ProductoOverlayViewModel(
        ProductoApiService apiProducto,
        CategoriaApiService apiCategoria,
        NotificationService notify,
        ProductoDto? producto = null)
    {
        _apiProducto = apiProducto;
        _apiCategoria = apiCategoria;
        _notify = notify;

        Producto = producto;
    }



    [ObservableProperty]
    private ProductoDto? producto;

    [ObservableProperty]
    private ObservableCollection<CategoriaDto> categorias = [];

    [ObservableProperty]
    private CategoriaDto? categoriaSeleccionada;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string codigoBarra = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string descripcion = "";

    [ObservableProperty]
    private decimal precioCompra;

    [ObservableProperty]
    private decimal precioVenta;

    [ObservableProperty]
    private int existencia;

    [ObservableProperty]
    private bool estado = true;

    [ObservableProperty]
    private DateTime fechaRegistro = DateTime.Today;

    [ObservableProperty]
    private bool isLoading;


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


    public async Task InicializarAsync()
    {
        await CargarCategoriasAsync();

        if (Producto != null)
        {
            Codigo = Producto.Codigo;
            CodigoBarra = Producto.CodigoBarra;
            Nombre = Producto.Nombre;
            Descripcion = Producto.Descripcion;

            PrecioCompra = Producto.PrecioCompra;
            PrecioVenta = Producto.PrecioVenta;

            Existencia = Producto.Existencia;
            Estado = Producto.Estado;

            FechaRegistro = Producto.FechaRegistro;

            CategoriaSeleccionada =
                Categorias.FirstOrDefault(
                    x => x.CategoriaId == Producto.CategoriaId);
        }
    }


    [RelayCommand]
    private async Task CargarCategoriasAsync()
    {
        try
        {
            IsLoading = true;

            var res =
                await _apiCategoria.ObtenerCategoriasAsync();

            if (!res.Success)
            {
                _notify.Error(
                    res.Message ?? "Error al cargar categorías");

                return;
            }

            Categorias =
                new ObservableCollection<CategoriaDto>(
                    res.Data ?? []);
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
    private async Task GuardarAsync()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                _notify.Warning(
                    "El nombre es obligatorio");

                return;
            }

            if (CategoriaSeleccionada == null)
            {
                _notify.Warning(
                    "Debe seleccionar una categoría");

                return;
            }

            if (PrecioCompra <= 0)
            {
                _notify.Warning(
                    "El precio de compra debe ser mayor a cero");

                return;
            }

            if (PrecioVenta <= 0)
            {
                _notify.Warning(
                    "El precio de venta debe ser mayor a cero");

                return;
            }

            if (PrecioVenta <= PrecioCompra)
            {
                _notify.Warning(
                    "El precio de venta debe ser mayor que el de compra");

                return;
            }

            if (Existencia < 0)
            {
                _notify.Warning(
                    "La existencia no puede ser negativa");

                return;
            }


            if (Producto != null)
            {
                var dto =
                    new ActualizarProductoDto
                    {
                        ProductoId = Producto.ProductoId,
                        CodigoBarra = CodigoBarra.Trim(),
                        Nombre = Nombre.Trim(),
                        Descripcion = Descripcion.Trim(),

                        CategoriaId =
                            CategoriaSeleccionada.CategoriaId,

                        PrecioCompra = PrecioCompra,
                        PrecioVenta = PrecioVenta,
                        Existencia = Existencia,
                        Estado = Estado
                    };

                var res =
                    await _apiProducto.ActualizarProductoAsync(
                        Producto.ProductoId,
                        dto);

                if (!res.Success)
                {
                    _notify.Error(
                        res.Message ?? "Error al actualizar producto");

                    return;
                }

                _notify.Success(
                    res.Message ?? "Producto actualizado");
            }


            else
            {
                var dto =
                    new CrearProductoDto
                    {
                        CodigoBarra = CodigoBarra.Trim(),
                        Nombre = Nombre.Trim(),
                        Descripcion = Descripcion.Trim(),

                        CategoriaId =
                            CategoriaSeleccionada.CategoriaId,

                        PrecioCompra = PrecioCompra,
                        PrecioVenta = PrecioVenta,
                        Existencia = Existencia,
                        Estado = Estado,

                        FechaRegistro = DateTime.Now
                    };

                var res =
                    await _apiProducto.CrearProductoAsync(dto);

                if (!res.Success)
                {
                    _notify.Error(
                        res.Message ?? "Error al crear producto");

                    return;
                }

                _notify.Success(
                    res.Message ?? "Producto creado");
            }

            Limpiar();

            _tcs.TrySetResult(true);
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
    private void Cancelar()
    {
        Limpiar();

        _tcs.TrySetResult(false);
    }


    private void Limpiar()
    {
        Producto = null;

        Codigo = "";
        CodigoBarra = "";

        Nombre = "";
        Descripcion = "";

        PrecioCompra = 0;
        PrecioVenta = 0;

        Existencia = 0;

        Estado = true;

        FechaRegistro = DateTime.Now;

        CategoriaSeleccionada = null;
    }
}