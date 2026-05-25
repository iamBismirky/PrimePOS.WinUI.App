using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.Contracts.DTOs.Producto;

namespace PrimePOS.BLL.Validators
{
    public class ProductoValidator
    {
        public static void ValidarCrearProducto(CrearProductoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre es obligatorio.", StatusCodes.Status400BadRequest);

            if (dto.CategoriaId <= 0)
                throw new BusinessException("Seleccione una categoría.", StatusCodes.Status400BadRequest);

            if (dto.PrecioCompra <= 0)
                throw new BusinessException("El precio de compra debe ser mayor que 0.", StatusCodes.Status400BadRequest);

            if (dto.Existencia <= 0)
                throw new BusinessException("La existencia no puede ser negativa.", StatusCodes.Status400BadRequest);

            if (dto.PorcentajeGananciaMinorista < 0)
                throw new BusinessException("El porcentaje de ganancia no puede ser negativo.", StatusCodes.Status400BadRequest);

            if (dto.ItbisPorcentaje < 0)
                throw new BusinessException("El ITBIS no puede ser negativo.", StatusCodes.Status400BadRequest);
        }

        public static void ValidarActualizar(ActualizarProductoDto dto)
        {
            if (dto.ProductoId <= 0)
                throw new BusinessException("El producto no es válido.", StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre del producto es obligatorio.", StatusCodes.Status400BadRequest);

            if (dto.CategoriaId <= 0)
                throw new BusinessException("Seleccione una categoría.", StatusCodes.Status400BadRequest);

            if (dto.PrecioCompra <= 0)
                throw new BusinessException("El precio de compra debe ser mayor que 0.", StatusCodes.Status400BadRequest);

            if (dto.Existencia <= 0)
                throw new BusinessException("La existencia no puede ser negativa.", StatusCodes.Status400BadRequest);

            if (dto.PorcentajeGananciaMinorista < 0)
                throw new BusinessException("El porcentaje de ganancia no puede ser negativo.", StatusCodes.Status400BadRequest);

            if (dto.ItbisPorcentaje < 0)
                throw new BusinessException("El ITBIS no puede ser negativo.", StatusCodes.Status400BadRequest);
        }

        public static void ValidarEliminar(int productoId)
        {
            if (productoId <= 0)
                throw new BusinessException("El producto no es válido.", StatusCodes.Status400BadRequest);
        }
    }
}