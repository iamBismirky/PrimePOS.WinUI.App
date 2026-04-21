using PrimePOS.BLL.Exceptions;
using PrimePOS.Contracts.DTOs.Usuario;

namespace PrimePOS.BLL.Validators
{
    public class UsuarioValidator
    {
        public static void ValidarCrear(CrearUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre es obligatorio.", 400);

            if (string.IsNullOrWhiteSpace(dto.Apellidos))
                throw new BusinessException("El apellido es obligatorio.", 400);

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessException("El nombre de usuario es obligatorio.", 400);

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new BusinessException("La contraseña es obligatoria.", 400);

            if (dto.RolId <= 0)
                throw new BusinessException("Debe seleccionar un rol.", 400);
        }

        public static void ValidarActualizar(ActualizarUsuarioDto dto)
        {
            if (dto.UsuarioId <= 0)
                throw new BusinessException("El usuario no es válido.", 400);

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new BusinessException("El nombre es obligatorio.", 400);

            if (string.IsNullOrWhiteSpace(dto.Apellidos))
                throw new BusinessException("El apellido es obligatorio.", 400);

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessException("El nombre de usuario es obligatorio.", 400);

            if (dto.RolId <= 0)
                throw new BusinessException("Debe seleccionar un rol.", 400);
        }

        public static void ValidarEliminar(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario no es válido.", 400);
        }
    }
}