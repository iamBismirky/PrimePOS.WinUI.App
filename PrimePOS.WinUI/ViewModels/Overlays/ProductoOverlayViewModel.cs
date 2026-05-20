using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels.Overlays;

public partial class ProductoOverlayViewModel : ObservableObject, IOverlayViewModel
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


    [ObservableProperty] private string codigo = "--------";
    [ObservableProperty] private string codigoBarra = "";
    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string descripcion = "";


    [ObservableProperty] private decimal precioCompra;


    [ObservableProperty] private int existencia;
    [ObservableProperty] private bool estado = true;
    [ObservableProperty] private DateTime fechaRegistro = DateTime.Today;


    [ObservableProperty] private decimal porcentajeGanancia = 35;
    [ObservableProperty] private bool aplicaItbis = true;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool precioAutomatico = true;


    [ObservableProperty] private decimal precioBase;

    [ObservableProperty] private decimal itbis;

    [ObservableProperty] private decimal precioFinal;
    [ObservableProperty] private decimal ganancia;


    private const decimal ITBIS_GLOBAL = 18;



    partial void OnPrecioCompraChanged(decimal value)
    => RecalcularPrecios();

    partial void OnPorcentajeGananciaChanged(decimal value)
        => RecalcularPrecios();

    partial void OnAplicaItbisChanged(bool value)
        => RecalcularPrecios();

    partial void OnPrecioAutomaticoChanged(bool value)
    {
        if (value)
        {
            PorcentajeGanancia = 35;
            return;
        }
        RecalcularPrecios();
    }
    private void RecalcularPrecios()
    {
        PrecioBase = PrecioCompra + (PrecioCompra * PorcentajeGanancia / 100);

        Itbis = AplicaItbis
            ? PrecioBase * (ITBIS_GLOBAL / 100)
            : 0;

        PrecioFinal = PrecioBase + Itbis;

        Ganancia = PrecioCompra * (PorcentajeGanancia / 100);
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
            Existencia = Producto.Existencia;
            Estado = Producto.Estado;

            CategoriaSeleccionada =
                Categorias.FirstOrDefault(x =>
                    x.CategoriaId == Producto.CategoriaId);
        }
    }





    [RelayCommand]
    private async Task CargarCategoriasAsync()
    {
        try
        {
            IsLoading = true;

            var res = await _apiCategoria.ObtenerCategoriasAsync();

            if (!res.Success)
            {
                _notify.Error(res.Message ?? "Error al cargar categorías");
                return;
            }

            Categorias = new ObservableCollection<CategoriaDto>(res.Data ?? []);
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
                _notify.Warning("El nombre es obligatorio");
                return;
            }

            if (CategoriaSeleccionada == null)
            {
                _notify.Warning("Debe seleccionar una categoría");
                return;
            }

            if (PrecioCompra <= 0)
            {
                _notify.Warning("Precio de compra inválido");
                return;
            }

            if (Existencia <= 0)
            {
                _notify.Warning("Existencia inválida");
                return;
            }


            if (Producto != null)
            {
                var dto = new ActualizarProductoDto
                {
                    ProductoId = Producto.ProductoId,
                    CodigoBarra = CodigoBarra.Trim(),
                    Nombre = Nombre.Trim(),
                    Descripcion = Descripcion.Trim(),
                    CategoriaId = CategoriaSeleccionada.CategoriaId,
                    PorcentajeGanancia = PorcentajeGanancia,
                    AplicaItbis = AplicaItbis,
                    ItbisPorcentaje = ITBIS_GLOBAL,
                    ItbisMonto = Itbis,
                    PrecioCompra = PrecioCompra,
                    PrecioVenta = PrecioFinal,
                    Existencia = Existencia,
                    Estado = Estado
                };

                var res = await _apiProducto.ActualizarProductoAsync(Producto.ProductoId, dto);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al actualizar producto");
                    return;
                }

                _notify.Success("Producto actualizado");
            }

            else
            {
                var dto = new CrearProductoDto
                {
                    CodigoBarra = CodigoBarra.Trim(),
                    Nombre = Nombre.Trim(),
                    Descripcion = Descripcion.Trim(),
                    CategoriaId = CategoriaSeleccionada.CategoriaId,
                    PorcentajeGanancia = PorcentajeGanancia,
                    PrecioCompra = PrecioCompra,
                    PrecioVenta = PrecioFinal,
                    AplicaItbis = AplicaItbis,
                    ItbisPorcentaje = ITBIS_GLOBAL,
                    ItbisMonto = Itbis,
                    Existencia = Existencia,
                    Estado = Estado,
                    FechaRegistro = DateTime.Now
                };

                var res = await _apiProducto.CrearProductoAsync(dto);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al crear producto");
                    return;
                }

                _notify.Success("Producto creado");
            }

            Limpiar();
            Close(true);
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
        Close(false);
    }
    public void Close(bool result)
    {
        _tcs.TrySetResult(result);
    }

    private void Limpiar()
    {
        Producto = null;

        Codigo = "";
        CodigoBarra = "";
        Nombre = "";
        Descripcion = "";

        PrecioCompra = 0;
        Existencia = 0;
        Estado = true;

        PorcentajeGanancia = 35;
        AplicaItbis = true;

        CategoriaSeleccionada = null;
    }
}