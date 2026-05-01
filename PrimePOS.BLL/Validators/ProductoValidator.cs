using PrimePOS.BLL.Exceptions;
using PrimePOS.Contracts.DTOs.Producto;

namespace PrimePOS.BLL.Validators
{
    public class ProductoValidator
    {
        public static void ValidarCrearProducto(CrearProductoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre es obligatorio.", 400);

            if (dto.CategoriaId <= 0)
                throw new BusinessException("Seleccione una categoría.", 400);

            if (dto.PrecioCompra <= 0)
                throw new BusinessException("El precio de compra debe ser mayor que 0.", 400);

            if (dto.PrecioVenta <= 0)
                throw new BusinessException("El precio de venta debe ser mayor que 0.", 400);

            if (dto.PrecioVenta <= dto.PrecioCompra)
                throw new BusinessException("El precio de venta debe ser mayor que el precio de compra.", 400);

            if (dto.Existencia <= 0)
                throw new BusinessException("La existencia debe ser mayor que 0.", 400);
        }

        public static void ValidarActualizar(ActualizarProductoDto dto)
        {
            if (dto.ProductoId <= 0)
                throw new BusinessException("El producto no es válido.", 400);

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre del producto es obligatorio.", 400);

            if (dto.CategoriaId <= 0)
                throw new BusinessException("Seleccione una categoría.", 400);

            if (dto.PrecioCompra <= 0)
                throw new BusinessException("El precio de compra debe ser mayor que 0.", 400);

            if (dto.PrecioVenta <= 0)
                throw new BusinessException("El precio de venta debe ser mayor que 0.", 400);

            if (dto.PrecioVenta <= dto.PrecioCompra)
                throw new BusinessException("El precio de venta debe ser mayor que el precio de compra.", 400);

            if (dto.Existencia < 0)
                throw new BusinessException("La existencia no puede ser negativa.", 400);
        }

        public static void ValidarEliminar(int productoId)
        {
            if (productoId <= 0)
                throw new BusinessException("El producto no es válido.", 400);
        }
    }
}