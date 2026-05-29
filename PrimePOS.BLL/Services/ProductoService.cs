using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Validators;
using PrimePOS.Contracts.DTOs.Producto;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository repository)
    {
        _productoRepository = repository;
    }

    public async Task CrearProductoAsync(CrearProductoDto dto)
    {
        ProductoValidator.ValidarCrearProducto(dto);

        var existe = await _productoRepository
            .ExisteCodigoONombreAsync(dto.CodigoBarra.Trim(), dto.Nombre.Trim());

        if (existe)
            throw new BusinessException("Ya existe un producto con ese nombre o código de barras.", StatusCodes.Status409Conflict);

        var producto = new Producto
        {
            CodigoBarra = dto.CodigoBarra,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CategoriaId = dto.CategoriaId,
            PrecioCompra = dto.PrecioCompra,
            PorcentajeGananciaMinorista = dto.PorcentajeGananciaMinorista,
            PorcentajeGananciaMayorista = dto.PorcentajeGananciaMayorista,
            AplicaItbis = dto.AplicaItbis,
            ItbisPorcentaje = dto.ItbisPorcentaje,
            Existencia = dto.Existencia,
            Estado = dto.Estado,
            FechaRegistro = DateTime.Now,
        };

        RecalcularPrecios(producto, dto.ItbisPorcentaje);

        _productoRepository.Crear(producto);
        await _productoRepository.GuardarCambiosAsync();

        producto.Codigo = GenerarCodigoProducto(producto.ProductoId);
        _productoRepository.Actualizar(producto);

        await _productoRepository.GuardarCambiosAsync();
    }

    public async Task ActualizarProductoAsync(ActualizarProductoDto dto)
    {
        ProductoValidator.ValidarActualizar(dto);

        var producto = await _productoRepository.ObtenerPorIdAsync(dto.ProductoId);

        if (producto == null)
            throw new BusinessException("El producto no existe.", StatusCodes.Status404NotFound);

        producto.CodigoBarra = dto.CodigoBarra;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.CategoriaId = dto.CategoriaId;
        producto.PrecioCompra = dto.PrecioCompra;
        producto.PorcentajeGananciaMinorista = dto.PorcentajeGananciaMinorista;
        producto.PorcentajeGananciaMayorista = dto.PorcentajeGananciaMayorista;
        producto.AplicaItbis = dto.AplicaItbis;
        producto.ItbisPorcentaje = dto.ItbisPorcentaje;
        producto.Existencia = dto.Existencia;
        producto.Estado = dto.Estado;

        RecalcularPrecios(producto, dto.ItbisPorcentaje);

        _productoRepository.Actualizar(producto);
        await _productoRepository.GuardarCambiosAsync();
    }

    public async Task EliminarProductoAsync(int productoId)
    {
        ProductoValidator.ValidarEliminar(productoId);

        var producto = await _productoRepository.ObtenerPorIdAsync(productoId);

        if (producto == null)
            throw new BusinessException("El producto no existe.", StatusCodes.Status404NotFound);

        _productoRepository.Eliminar(producto);
        await _productoRepository.GuardarCambiosAsync();
    }

    public async Task DesactivarProductoAsync(int productoId)
    {
        ProductoValidator.ValidarEliminar(productoId);

        var producto = await _productoRepository.ObtenerPorIdAsync(productoId);

        if (producto == null)
            throw new BusinessException("El producto no existe.", StatusCodes.Status404NotFound);

        producto.Estado = false;

        _productoRepository.Actualizar(producto);
        await _productoRepository.GuardarCambiosAsync();
    }


    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id);

        if (producto == null)
            throw new BusinessException("El producto no existe.", StatusCodes.Status404NotFound);

        return new ProductoDto
        {
            ProductoId = producto.ProductoId,
            Codigo = producto.Codigo,
            CodigoBarra = producto.CodigoBarra,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = producto.Categoria?.Nombre ?? "",
            PrecioCompra = producto.PrecioCompra,
            PrecioMinorista = producto.PrecioMinorista,
            PrecioMayorista = producto.PrecioMayorista,
            PorcentajeGananciaMinorista = producto.PorcentajeGananciaMinorista,
            PorcentajeGananciaMayorista = producto.PorcentajeGananciaMayorista,
            Existencia = producto.Existencia,
            Estado = producto.Estado,
            FechaRegistro = producto.FechaRegistro,
            ItbisPorcentaje = producto.ItbisPorcentaje,
            AplicaItbis = producto.AplicaItbis
        };
    }

    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var productos = await _productoRepository.ObtenerTodosAsync();

        return productos.Select(p => new ProductoDto
        {
            ProductoId = p.ProductoId,
            Codigo = p.Codigo,
            CodigoBarra = p.CodigoBarra,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre ?? "",
            PrecioCompra = p.PrecioCompra,
            PrecioMinorista = p.PrecioMinorista,
            PrecioMayorista = p.PrecioMayorista,
            PorcentajeGananciaMinorista = p.PorcentajeGananciaMinorista,
            PorcentajeGananciaMayorista = p.PorcentajeGananciaMayorista,
            Existencia = p.Existencia,
            Estado = p.Estado,
            FechaRegistro = p.FechaRegistro,
            ItbisPorcentaje = p.ItbisPorcentaje,
            AplicaItbis = p.AplicaItbis
        }).ToList();
    }

    private string GenerarCodigoProducto(int productoId)
    {
        return $"PROD-{productoId:D6}";
    }

    public async Task<List<ProductoDto>> BuscarProductosAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return new List<ProductoDto>();

        var productos = await _productoRepository.BuscarAsync(texto);

        if (productos == null)
            throw new BusinessException(
                "Producto no encontrado.",
                StatusCodes.Status404NotFound);


        return productos.Select(p => new ProductoDto
        {
            ProductoId = p.ProductoId,
            Codigo = p.Codigo,
            CodigoBarra = p.CodigoBarra,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre ?? "",
            PrecioCompra = p.PrecioCompra,
            PrecioMinorista = p.PrecioMinorista,
            PrecioMayorista = p.PrecioMayorista,
            PorcentajeGananciaMinorista = p.PorcentajeGananciaMinorista,
            PorcentajeGananciaMayorista = p.PorcentajeGananciaMayorista,
            Existencia = p.Existencia,
            Estado = p.Estado,
            FechaRegistro = p.FechaRegistro,
            ItbisPorcentaje = p.ItbisPorcentaje,
            AplicaItbis = p.AplicaItbis
        }).ToList();
    }

    private void RecalcularPrecios(Producto producto, decimal itbisPorcentaje)
    {
        var compra = producto.PrecioCompra;

        var itbisRate = itbisPorcentaje / 100m;



        var precioBaseMinorista =
            compra + (compra * producto.PorcentajeGananciaMinorista / 100m);

        var itbisMinorista =
            producto.AplicaItbis
                ? precioBaseMinorista * itbisRate
                : 0m;

        producto.PrecioMinorista = precioBaseMinorista;



        var precioBaseMayorista =
            compra + (compra * producto.PorcentajeGananciaMayorista / 100m);

        var itbisMayorista =
            producto.AplicaItbis
                ? precioBaseMayorista * itbisRate
                : 0m;

        producto.PrecioMayorista = precioBaseMayorista;
    }
}