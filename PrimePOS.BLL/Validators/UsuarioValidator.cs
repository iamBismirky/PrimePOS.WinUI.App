
using PrimePOS.BLL.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.BLL.Validators
{
    public class UsuarioValidator
    {
        public static void ValidarCrear(CrearUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new Exception("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("La contraseña es obligatoria.");

            if (dto.RolId <= 0)
                throw new Exception("Seleccione un rol.");

            
        }
        public static void ValidarActualizar(ActualizarUsuarioDto dto)
        {
            if (dto.UsuarioId <= 0)
                throw new Exception("El usuario no es válido.");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new Exception("El nombre de usuario es obligatorio.");

            if (dto.RolId <= 0)
                throw new Exception("Seleccione un rol.");
        }
        public static void ValidarEliminar(int productoId)
        {
            if (productoId <= 0)
                throw new Exception("El producto no es válido.");
        }
    }
}
