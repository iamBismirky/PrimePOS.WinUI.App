using PrimePOS.BLL.DTOs;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.BLL.Security;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository repository)
        {
            _usuarioRepository = repository;
        }

        public async Task CrearUsuarioAsync(CrearUsuarioDto dto)
        {
            await ValidarUsuario(dto);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Username = dto.Username,
                Password = PasswordService.Hash(dto.Password),
                Estado = dto.Estado,
                RolId = dto.RolId
            };

            _usuarioRepository.Crear(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
            
        }

        

        public async Task ActualizarUsuarioAsync(ActualizarUsuarioDto dto)
        {
            
            var usuario = await _usuarioRepository.ObtenerPorId(dto.UsuarioId)
                ?? throw new Exception("Usuario no encontrado. Seleccione uno.");

            if (await _usuarioRepository.ExisteUsernameAsync(dto.Username, dto.UsuarioId))
                throw new Exception("Ya existe un usuario con ese username.");

            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Username = dto.Username;
            usuario.Estado = dto.Estado;
            usuario.RolId = dto.RolId;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
            
        }

        public async Task<bool> EliminarUsuarioAsync(EliminarUsuarioDto dto)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(dto.UsuarioId)
                ?? throw new Exception("Usuario no encontrado. Seleccione uno");

            _usuarioRepository.Eliminar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
            return true;
        }

        public async Task CambiarEstadoAsync(int id, bool nuevoEstado)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(id)
                ?? throw new Exception("Usuario no encontrado.");

            usuario.Estado = nuevoEstado;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(UsuarioDto dto)
        {
            return await _usuarioRepository.ObtenerPorId(dto.UsuarioId);
        }
        public async Task<List<UsuarioDto>> ListarUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.ListarUsuariosAsync();

            return usuarios.Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Apellidos = u.Apellidos,
                Username = u.Username,
                RolId = u.RolId,
                Estado = u.Estado,
                FechaRegistro = u.FechaRegistro
            }).ToList();
        }
        public async Task CambiarContraseñaAsync(int id, string contraseñaActual, string contraseñaNueva, string confirmacion)
        {
            if (string.IsNullOrWhiteSpace(contraseñaActual))
                throw new Exception("La contraseña actual es obligatoria.");

            if (string.IsNullOrWhiteSpace(contraseñaNueva))
                throw new Exception("La nueva contraseña es obligatoria.");

            if (string.IsNullOrWhiteSpace(confirmacion))
                throw new Exception("Confirme la contraseña.");

            if (contraseñaNueva != confirmacion)
                throw new Exception("La confirmación no coincide con la nueva contraseña.");

            var usuario = await _usuarioRepository.ObtenerPorId(id)
                ?? throw new Exception("Usuario no encontrado.");

            bool esValida = PasswordService.Verify(contraseñaActual, usuario.Password);

            if (!esValida)
                throw new Exception("La contraseña actual es incorrecta.");

            if (PasswordService.Verify(contraseñaNueva, usuario.Password))
                throw new Exception("La nueva contraseña no puede ser igual a la actual.");

            usuario.Password = PasswordService.Hash(contraseñaNueva);

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(AutenticarUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Usuario y clave obligatorios.");

            var usuario = await _usuarioRepository.ObtenerPorUsernameAsync(dto.Username)
                ?? throw new Exception("Usuario o contraseña incorrectos.");

            if (!usuario.Estado)
                throw new Exception("Usuario inactivo.");

            bool esValida = PasswordService.Verify(dto.Password, usuario.Password);

            if (!esValida)
                throw new Exception("Usuario o contraseña incorrectos.");

            return new AppSesionUsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                UsuarioNombre = usuario.Nombre +" "+ usuario.Apellidos,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre ?? ""
            };
        }

        
        private async Task ValidarUsuario(CrearUsuarioDto dto, bool esActualizacion = false)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Apellidos))
                throw new Exception("Los apellidos son obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new Exception("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("La contraseña es obligatoria.");

            if (dto.Password.Length < 6)
                throw new Exception("La contraseña debe tener mínimo 6 caracteres.");

            if (dto.RolId <= 0)
                throw new Exception("Debe seleccionar un rol.");

            var existeUsuario = await _usuarioRepository.ExisteUsernameAsync(dto.Username);
        }
    }
}


