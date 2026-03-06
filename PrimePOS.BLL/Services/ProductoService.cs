using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.BLL.Services;

public class ProductoService
{
    private readonly ProductoRepository _repository;

    public ProductoService(ProductoRepository repository)
    {
        _repository = repository;
    }
    public List<Producto> ListarProductos()
    {
        return _repository.Listar();
    }
    public void CrearProducto(string codigo, string? codigoBarra, string nombre,string descripcion, int categoriaId,decimal precioCompra, decimal precioVenta,int existencia, bool estado,int existenciaMinimo = 0)
    {
        ValidarProducto(codigo, nombre, descripcion, categoriaId,precioCompra, precioVenta, existencia);

        if (_repository.ExisteCodigo(codigo))
            throw new Exception("Ya existe un producto con ese código.");

        var producto = new Producto
        {
            Codigo = codigo.Trim(),
            CodigoBarra = string.IsNullOrWhiteSpace(codigoBarra) ? null : codigoBarra.Trim(),
            Nombre = nombre.Trim(),
            Descripcion = descripcion.Trim(),
            CategoriaId = categoriaId,
            PrecioCompra = precioCompra,
            PrecioVenta = precioVenta,
            Existencia = existencia,
            ExistenciaMinimo = existenciaMinimo,
            Estado = estado
        };

        _repository.Agregar(producto);
        _repository.GuardarCambios();
    }
    public void ActualizarProducto(int productoId, string codigo, string? codigoBarra,string nombre, string descripcion,int categoriaId, decimal precioCompra,decimal precioVenta, int existencia,bool estado)
    {
        if (productoId == 0)
            throw new Exception("Seleccione un producto.");

        ValidarProducto(codigo, nombre, descripcion, categoriaId, precioCompra, precioVenta, existencia);

        var producto = _repository.ObtenerPorId(productoId);

        if (producto == null)
            throw new Exception("Producto no encontrado.");

        if (_repository.ExisteCodigo(codigo, productoId))
            throw new Exception("Ya existe un producto con ese código.");

        producto.Codigo = codigo.Trim();
        producto.CodigoBarra = string.IsNullOrWhiteSpace(codigoBarra) ? null : codigoBarra.Trim();
        producto.Nombre = nombre.Trim();
        producto.Descripcion = descripcion.Trim();
        producto.CategoriaId = categoriaId;
        producto.PrecioCompra = precioCompra;
        producto.PrecioVenta = precioVenta;
        producto.Existencia = existencia;
        producto.Estado = estado;

        _repository.Actualizar(producto);
        _repository.GuardarCambios();

    }
    public void EliminarProducto(int productoId)
    {
        var producto = _repository.ObtenerPorId(productoId);

        if (producto == null)
            throw new Exception("Producto no encontrado.");

        _repository.Eliminar(producto);
        _repository.GuardarCambios();

    }
    public Producto? BuscarProducto(string buscar)
    {
        return _repository.BuscarPorCodigoONombre(buscar);
    }
    private void ValidarProducto(string codigo, string nombre,
                                 string descripcion, int categoriaId,
                                 decimal precioCompra, decimal precioVenta,
                                 int existencia)
    {
        if (string.IsNullOrWhiteSpace(codigo) ||
            string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(descripcion))
            throw new Exception("Todos los campos son obligatorios.");

        if (categoriaId == 0)
            throw new Exception("Seleccione una categoría.");

        if (precioCompra <= 0 || precioVenta <= 0)
            throw new Exception("Los precios deben ser mayores que cero.");

        if (precioVenta <= precioCompra)
            throw new Exception("El precio de venta debe ser mayor al precio de compra.");

        if (existencia < 0)
            throw new Exception("La existencia no puede ser negativa.");
    }
    public Producto? ObtenerProductoPorId(int id) 
    { 
        return _repository.ObtenerPorId(id);
    }
}
