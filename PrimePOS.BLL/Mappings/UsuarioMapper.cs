using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Mappings
{
    public static class UsuarioMapper
    {
        // Entity → DTO
        public static UsuarioDto? ToDto(Usuario usuario)
        {
            if (usuario == null) return null;

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Username = usuario.Username,
                RolId = usuario.RolId,
                Estado = usuario.Estado
            };
        }

        // CrearUsuarioDto → Entity
        public static Usuario ToEntity(CrearUsuarioDto dto)
        {
            return new Usuario
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Username = dto.Username,
                Password = dto.Password,
                RolId = dto.RolId,
                Estado = true
            };
        }

        // ActualizarUsuarioDto → Entity
        public static void UpdateEntity(Usuario usuario, ActualizarUsuarioDto dto)
        {
            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Username = dto.Username;
            usuario.RolId = dto.RolId;
            usuario.Estado = dto.Estado;
        }
        // Eliminar 
        public static void EliminarEntity(Usuario usuario, EliminarUsuarioDto dto)
        {
            usuario.UsuarioId = dto.UsuarioId;

        }
        public static void ListarEntity(Usuario usuario, ListarUsuarios dto)
        {
            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Username = dto.Username;
            usuario.RolId = dto.RolId;
            usuario.Estado = dto.Estado;
        }
    }
}
