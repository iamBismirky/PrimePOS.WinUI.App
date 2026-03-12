using PrimePOS.BLL.DTOs.Producto;
using PrimePOS.BLL.Validators;
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
        ProductoValidator.ValidarCrearProducto(dto);

        var existe = await _productoRepository.BuscarPorCodigoONombreAsync(dto.Nombre);

        if (existe != null) 
            throw new Exception("Ya existe un producto con este nombre");

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
            throw new Exception("Producto no existe.");

        
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
        ProductoValidator.ValidarEliminar(dto.ProductoId);

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
