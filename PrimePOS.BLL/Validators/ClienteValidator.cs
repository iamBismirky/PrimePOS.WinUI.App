using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.Contracts.DTOs.Cliente;

namespace PrimePOS.BLL.Validators
{
    public class ClienteValidator
    {
        public static void ValidarCrearCliente(CrearClienteDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre del cliente es obligatorio.", StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(dto.Documento))
                throw new BusinessException("El documento del cliente es obligatorio.", StatusCodes.Status400BadRequest);

            if (dto.TipoClienteId <= 0)
                throw new BusinessException("El tipo de cliente es obligatorio.", StatusCodes.Status400BadRequest);


        }
    }
}
