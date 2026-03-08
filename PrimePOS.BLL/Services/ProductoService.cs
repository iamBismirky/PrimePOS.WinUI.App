using PrimePOS.BLL.DTOs;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class ProductoService
{
    private readonly ProductoRepository _productoRepository;

    public ProductoService(ProductoRepository repository)
    {
        _productoRepository = repository;
    }
    public async Task<List<Producto>> ListarProductosAsync()
    {
        return await _productoRepository.ListarAsync();
    }
    public async Task CrearProducto(ProductoDto dto)
    {
        ValidarProducto(dto);

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
            ExistenciaMinimo = dto.ExistenciaMinimo,
            Estado = dto.Estado
        };

        _productoRepository.Crear(producto);
        await _productoRepository.GuardarCambiosAsync();
    }
    public async Task ActualizarProducto(ProductoDto dto)
    {
        if (dto.ProductoId == 0)
            throw new Exception("Seleccione un producto.");

        ValidarProducto(dto);

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
    public async Task EliminarProducto(ProductoDto dto)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(dto.ProductoId);

        if (producto == null)
            throw new Exception("Producto no encontrado.");

        _productoRepository.Eliminar(producto);
        await _productoRepository.GuardarCambiosAsync();

    }
    public async Task<Producto?> BuscarProducto(ProductoDto dto)
    {
        return await _productoRepository.BuscarPorCodigoONombreAsync(dto.Nombre);
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
    public async Task<Producto?> ObtenerProductoPorIdAsync(int id)
    {
        return await _productoRepository.ObtenerPorIdAsync(id);
    }
}
