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
            throw new BusinessException("Ya existe un producto con ese nombre o código de barras.", 400);

        var producto = new Producto
        {
            CodigoBarra = dto.CodigoBarra,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CategoriaId = dto.CategoriaId,
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            Existencia = dto.Existencia,
            Estado = dto.Estado,
            FechaRegistro = DateTime.Now,
        };

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
            throw new BusinessException("El producto no existe.", 404);

        producto.CodigoBarra = dto.CodigoBarra;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.CategoriaId = dto.CategoriaId;
        producto.PrecioCompra = dto.PrecioCompra;
        producto.PrecioVenta = dto.PrecioVenta;
        producto.Existencia = dto.Existencia;
        producto.Estado = dto.Estado;

        _productoRepository.Actualizar(producto);
        await _productoRepository.GuardarCambiosAsync();
    }

    public async Task EliminarProductoAsync(int productoId)
    {
        ProductoValidator.ValidarEliminar(productoId);

        var producto = await _productoRepository.ObtenerPorIdAsync(productoId);

        if (producto == null)
            throw new BusinessException("El producto no existe.", 404);

        _productoRepository.Eliminar(producto);
        await _productoRepository.GuardarCambiosAsync();
    }

    public async Task DesactivarProductoAsync(int productoId)
    {
        ProductoValidator.ValidarEliminar(productoId);

        var producto = await _productoRepository.ObtenerPorIdAsync(productoId);

        if (producto == null)
            throw new BusinessException("El producto no existe.", 404);

        producto.Estado = false;

        _productoRepository.Actualizar(producto);
        await _productoRepository.GuardarCambiosAsync();
    }


    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id);

        if (producto == null)
            throw new BusinessException("El producto no existe.", 404);

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
            PrecioVenta = producto.PrecioVenta,
            Existencia = producto.Existencia,
            Estado = producto.Estado,
            FechaRegistro = producto.FechaRegistro,
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
            PrecioVenta = p.PrecioVenta,
            Existencia = p.Existencia,
            Estado = p.Estado,
            FechaRegistro = p.FechaRegistro,
        }).ToList();
    }

    private string GenerarCodigoProducto(int productoId)
    {
        return $"PROD-{productoId:D4}";
    }
    public async Task<List<ProductoDto>> BuscarProductosAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return new List<ProductoDto>();

        var productos = await _productoRepository.BuscarAsync(texto);
        if (productos == null)
            throw new BusinessException("Producto no encontrado.", 404);

        return productos.Select(p => new ProductoDto
        {
            ProductoId = p.ProductoId,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta
        }).ToList();
    }
}