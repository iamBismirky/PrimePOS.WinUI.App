using PrimePOS.BLL.DTOs.Producto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.Validators
{
    public class ProductoValidator
    {
        public static void ValidarCrearProducto(CrearProductoDto dto) 
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre es obligatorio.");

            if (dto.CategoriaId <= 0)
                throw new Exception("Seleccione una categoría.");

            if (dto.PrecioCompra <= 0)
                throw new Exception("El precio de compra debe ser mayor que 0.");

            if (dto.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor que 0.");

            if (dto.PrecioVenta <= dto.PrecioCompra)
                throw new Exception("El precio de venta debe ser mayor que el precio de compra.");

            if (dto.Existencia <= 0)
                throw new Exception("La existencia debe ser mayor que 0.");
        }
        public static void ValidarActualizar(ActualizarProductoDto dto)
        {
            if (dto.ProductoId <= 0)
                throw new Exception("El producto no es válido.");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del producto es obligatorio.");

            if (dto.CategoriaId <= 0)
                throw new Exception("Seleccione una categoría.");

            if (dto.PrecioCompra <= 0)
                throw new Exception("El precio de compra debe ser mayor que 0.");

            if (dto.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor que 0.");

            if (dto.PrecioVenta <= dto.PrecioCompra)
                throw new Exception("El precio de venta debe ser mayor que el precio de compra.");

            if (dto.Existencia < 0)
                throw new Exception("La existencia no puede ser negativa.");
        }
        public static void ValidarEliminar(int productoId)
        {
            if (productoId <= 0)
                throw new Exception("El producto no es válido.");
        }

    }
    
}
