using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using PrimePOS.WinUI.Views.Overlays;
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
    private readonly OverlayService _overlayService;

    private readonly TaskCompletionSource<bool> _tcs = new();
    public Task<bool> WaitTask => _tcs.Task;

    public ProductoOverlayViewModel(
        ProductoApiService apiProducto,
        CategoriaApiService apiCategoria,
        NotificationService notify,
        OverlayService overlay)
    {
        _apiProducto = apiProducto;
        _apiCategoria = apiCategoria;
        _notify = notify;
        _overlayService = overlay;
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


    [ObservableProperty] private decimal porcentajeGananciaMinorista = 35;
    [ObservableProperty] private decimal porcentajeGananciaMayorista = 0;
    [ObservableProperty] private bool aplicaItbis = true;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool precioAutomatico = true;


    [ObservableProperty] private decimal precioBase;
    [ObservableProperty] private decimal precioBaseMayorista;

    [ObservableProperty] private decimal itbis;

    [ObservableProperty] private decimal precioFinalMinorista;
    [ObservableProperty] private decimal precioFinalMayorista;
    [ObservableProperty] private decimal ganancia;
    [ObservableProperty] private decimal gananciaMayorista;
    [ObservableProperty] private decimal precioMayorista;


    private const decimal ITBIS_GLOBAL = 18;



    partial void OnPrecioCompraChanged(decimal value)
    => RecalcularPrecios();

    partial void OnPorcentajeGananciaMinoristaChanged(decimal value)
    {
        if (PrecioAutomatico)
        {
            PorcentajeGananciaMayorista = value / 2m;
        }

        RecalcularPrecios();
    }
    partial void OnPorcentajeGananciaMayoristaChanged(decimal value)
    {
        RecalcularPrecios();
    }
    partial void OnAplicaItbisChanged(bool value)
        => RecalcularPrecios();
    public decimal PorcentajeMayoristaCalculado
    => PorcentajeGananciaMinorista / 2;


    partial void OnPrecioAutomaticoChanged(bool value)
    {
        if (value)
        {
            AplicarReglasPorcentaje();
        }

        RecalcularPrecios();

    }
    private void AplicarReglasPorcentaje()
    {
        if (PrecioAutomatico)
        {
            PorcentajeGananciaMinorista = 35;
            PorcentajeGananciaMayorista = PorcentajeGananciaMinorista / 2m;
        }
    }
    private void RecalcularPrecios()
    {
        //AplicarReglasPorcentaje();
        var compra = PrecioCompra;

        // =========================
        // MINORISTA
        // =========================

        PrecioBase =
            compra +
            (compra * PorcentajeGananciaMinorista / 100m);

        var itbisMinorista =
            AplicaItbis
                ? PrecioBase * (ITBIS_GLOBAL / 100m)
                : 0m;

        Itbis = itbisMinorista;

        PrecioFinalMinorista =
            PrecioBase + itbisMinorista;

        Ganancia =
            PrecioBase - compra;

        // =========================
        // MAYORISTA
        // =========================

        PrecioBaseMayorista =
            compra +
            (compra * PorcentajeGananciaMayorista / 100m);

        var itbisMayorista =
            AplicaItbis
                ? PrecioBaseMayorista * (ITBIS_GLOBAL / 100m)
                : 0m;

        PrecioFinalMayorista =
            PrecioBaseMayorista + itbisMayorista;

        GananciaMayorista =
            PrecioBaseMayorista - compra;
    }


    public async Task InicializarAsync(ProductoDto? producto)
    {
        AplicarReglasPorcentaje();
        await CargarCategoriasAsync();


        if (producto != null)
        {

            Producto = producto;
            Codigo = producto.Codigo;
            CodigoBarra = producto.CodigoBarra;
            Nombre = producto.Nombre;
            Descripcion = producto.Descripcion;

            PrecioCompra = producto.PrecioCompra;
            Existencia = producto.Existencia;
            Estado = producto.Estado;


            CategoriaSeleccionada =
                Categorias.FirstOrDefault(x =>
                    x.CategoriaId == producto.CategoriaId);

            PorcentajeGananciaMinorista = producto.PorcentajeGananciaMinorista;
            PorcentajeGananciaMayorista = producto.PorcentajeGananciaMayorista;
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
    private async Task NuevaCategoriaAsync()
    {
        var vm = App.Services.GetRequiredService<CategoriaOverlayViewModel>();
        var overlay = new CategoriaOverlay(vm);

        var creado = await _overlayService.ShowAsync(overlay, vm);

        if (creado)
            await CargarCategoriasAsync();

        return;
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
                    PrecioCompra = PrecioCompra,
                    PorcentajeGananciaMinorista = PorcentajeGananciaMinorista,
                    PorcentajeGananciaMayorista = PorcentajeGananciaMayorista,
                    AplicaItbis = AplicaItbis,
                    ItbisPorcentaje = ITBIS_GLOBAL,
                    Existencia = Existencia,
                    Estado = Estado
                };

                var res = await _apiProducto.ActualizarProductoAsync(Producto.ProductoId, dto);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al actualizar producto");
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
                    CategoriaId = CategoriaSeleccionada.CategoriaId,
                    PrecioCompra = PrecioCompra,
                    PorcentajeGananciaMinorista = PorcentajeGananciaMinorista,
                    PorcentajeGananciaMayorista = PorcentajeGananciaMayorista,
                    AplicaItbis = AplicaItbis,
                    ItbisPorcentaje = ITBIS_GLOBAL,
                    Existencia = Existencia,
                    Estado = Estado
                };

                var res = await _apiProducto.CrearProductoAsync(dto);

                if (!res.Success)
                {
                    _notify.Error(res.Message ?? "Error al crear producto");
                    return;
                }

                _notify.Success(res.Message);
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

        PorcentajeGananciaMinorista = 35;
        PorcentajeGananciaMayorista = 0;
        AplicaItbis = true;

        CategoriaSeleccionada = null;
    }
}