using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System.Reflection.Metadata.Ecma335;

namespace PrimePOS.BLL.Services;

public class ProductoService
{
    private readonly ProductoRepository _productoRepository;

    public ProductoService(ProductoRepository repository)
    {
        _productoRepository = repository;
    }
    public async Task CrearProductoAsync(CrearProductoDto dto)
    {
        

        var producto = new Producto
        {
            Codigo = dto.Codigo,
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
        if (dto.ProductoId == 0)
            throw new Exception("Seleccione un producto.");

        

        var producto = await _productoRepository.ObtenerPorIdAsync(dto.ProductoId);

        if (producto == null)
            throw new Exception("Producto no encontrado.");

        var ExisteProducto = await _productoRepository.ExisteCodigoAsync(dto.Codigo, dto.ProductoId);
        if (!ExisteProducto) throw new Exception("Ya existe un producto con ese codigo");

        producto.Codigo = dto.Codigo;
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
    public async Task EliminarProductoAsync(EliminarProductoDto dto)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(dto.ProductoId);

        if (producto == null)
            throw new Exception("Producto no encontrado.");

        _productoRepository.Eliminar(producto);
        await _productoRepository.GuardarCambiosAsync();

    }
    public async Task<List<ProductoDto>> BuscarProductoCodigoONombreListAsync(string buscar)
    {
        var producto = await _productoRepository.BuscarPorCodigoONombreListAsync(buscar);
        

        return producto.Select(p => new ProductoDto
        {
            ProductoId = p.ProductoId,
            Nombre = p.Nombre,
            Codigo = p.Codigo,
            PrecioVenta = p.PrecioVenta
        }).ToList();
    
    }
    public async Task<ProductoDto?> BuscarProductoCodigoONombreAsync(string buscar)
    {
        var producto = await _productoRepository.BuscarPorCodigoONombreAsync(buscar);
        if (producto == null)
            return null;

        return new ProductoDto
        {
            ProductoId = producto.ProductoId,
            Nombre = producto.Nombre,
            Codigo = producto.Codigo,
            PrecioVenta = producto.PrecioVenta
        };

    }
    private void ValidarProducto(ProductoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Codigo) ||
            string.IsNullOrWhiteSpace(dto.Nombre) ||
            string.IsNullOrWhiteSpace(dto.Descripcion))
            throw new Exception("Todos los campos son obligatorios.");

        if (dto.CategoriaId == 0)
            throw new Exception("Seleccione una categoría.");

        if (dto.PrecioCompra <= 0 || dto.PrecioVenta <= 0)
            throw new Exception("Los precios deben ser mayores que cero.");

        if (dto.PrecioVenta <= dto.PrecioCompra)
            throw new Exception("El precio de venta debe ser mayor al precio de compra.");

        if (dto.Existencia < 0)
            throw new Exception("La existencia no puede ser negativa.");
    }
    public async Task<ProductoDto?> ObtenerProductoPorIdAsync(int id)
    {
        var producto =  await _productoRepository.ObtenerPorIdAsync(id);

        if (producto == null) return null;

        return new ProductoDto
        {
            Codigo = producto.Codigo,
            CodigoBarra = producto.CodigoBarra,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            CategoriaId = producto.CategoriaId,
            NombreCategoria = producto.Categoria?.Nombre ?? "",
            PrecioCompra = producto.PrecioCompra,
            PrecioVenta = producto.PrecioVenta,
            Existencia = producto.Existencia,
            Estado = producto.Estado,
            FechaRegistro = producto.FechaRegistro,

        };
    }

    public async Task<List<ProductoDto>> ListarProductosAsync()
    {
        var productos = await _productoRepository.ListarAsync();

        return productos.Select(p => new ProductoDto
        {
            ProductoId = p.ProductoId,
            Codigo = p.Codigo,
            CodigoBarra = p.CodigoBarra,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId= p.CategoriaId,
            NombreCategoria = p.Categoria?.Nombre ?? "",
            PrecioCompra = p.PrecioCompra,
            PrecioVenta = p.PrecioVenta,
            Existencia = p.Existencia,
            Estado = p.Estado,
            FechaRegistro = p.FechaRegistro,



        }).ToList();
    }
    private string GenerarCodigoProducto(int productoId)
    {
        return $"PRO-{productoId:D4}";
    }
}
