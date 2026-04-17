using PrimePOS.BLL.Exceptions;
using PrimePOS.Contracts.DTOs.Usuario;

namespace PrimePOS.BLL.Validators
{
    public class UsuarioValidator
    {
        public static void ValidarCrear(CrearUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre es obligatorio.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.Apellidos))
                throw new BusinessException("El apellido es obligatorio.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessException("El nombre de usuario es obligatorio.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new BusinessException("La contraseña es obligatoria.", "REQUIRED");

            if (dto.RolId <= 0)
                throw new BusinessException("Seleccione un rol.", "REQUIRED");


        }
        public static void ValidarActualizar(ActualizarUsuarioDto dto)
        {
            if (dto.UsuarioId <= 0)
                throw new BusinessException("El usuario no es válido.", "INVALID");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre es obligatorio.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.Apellidos))
                throw new BusinessException("El apellido es obligatorio.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessException("El nombre de usuario es obligatorio.", "REQUIRED");
            if (dto.RolId <= 0)
                throw new BusinessException("Seleccione un rol.", "REQUIRED");
        }
        public static void ValidarEliminar(int productoId)
        {
            if (productoId <= 0)
                throw new BusinessException("El producto no es válido.", "INVALID");
        }
    }
}
