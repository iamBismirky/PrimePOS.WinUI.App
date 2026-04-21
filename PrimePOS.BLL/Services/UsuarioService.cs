using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Security;
using PrimePOS.BLL.Validators;
using PrimePOS.Contracts.DTOs.Usuario;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtHelper _jwtHelper;

        public UsuarioService(IUsuarioRepository repository, JwtHelper jwtHelper)
        {
            _usuarioRepository = repository;
            _jwtHelper = jwtHelper;
        }

        public async Task CrearUsuarioAsync(CrearUsuarioDto dto)
        {
            UsuarioValidator.ValidarCrear(dto);

            if (await _usuarioRepository.ExisteUsernameAsync(dto.Username, null))
                throw new BusinessException("Ya existe un usuario con ese username.", 400);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Username = dto.Username,
                Password = PasswordService.Hash(dto.Password),
                Estado = dto.Estado,
                RolId = dto.RolId,
                FechaRegistro = DateTime.Now,
            };

            _usuarioRepository.Crear(usuario);
            await _usuarioRepository.GuardarCambiosAsync();

            usuario.Codigo = GenerarCodigoUsuario(usuario.UsuarioId);

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task ActualizarUsuarioAsync(ActualizarUsuarioDto dto)
        {
            UsuarioValidator.ValidarActualizar(dto);

            var usuario = await _usuarioRepository.ObtenerPorId(dto.UsuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario no existe.", 404);

            if (await _usuarioRepository.ExisteUsernameAsync(dto.Username, dto.UsuarioId))
                throw new BusinessException("Ya existe un usuario con ese username.", 400);

            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Username = dto.Username;
            usuario.Estado = dto.Estado;
            usuario.RolId = dto.RolId;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task EliminarUsuarioAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario no existe.", 404);

            _usuarioRepository.Eliminar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task DesactivarUsuarioAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario no existe.", 404);

            usuario.Estado = false;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task CambiarEstadoAsync(int id, bool nuevoEstado)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
                throw new BusinessException("El usuario no existe.", 404);

            usuario.Estado = nuevoEstado;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
                throw new BusinessException("El usuario no existe.", 404);

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Codigo = usuario.Codigo,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Username = usuario.Username,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre ?? "",
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<List<UsuarioDto>> ObtenerTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObtenerTodosAsync();

            return usuarios.Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Codigo = u.Codigo,
                Nombre = u.Nombre,
                Apellidos = u.Apellidos,
                Username = u.Username,
                RolId = u.RolId,
                RolNombre = u.Rol?.Nombre ?? "",
                Estado = u.Estado,
                FechaRegistro = u.FechaRegistro
            }).ToList();
        }

        public async Task CambiarPasswordAsync(int usuarioId, CambiarPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PasswordActual) ||
                string.IsNullOrWhiteSpace(dto.PasswordNueva) ||
                string.IsNullOrWhiteSpace(dto.Confirmar))
                throw new BusinessException("Todos los campos son obligatorios.", 400);

            if (dto.PasswordNueva != dto.Confirmar)
                throw new BusinessException("La confirmación no coincide.", 400);

            var usuario = await _usuarioRepository.ObtenerPorId(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario no existe.", 404);

            if (!PasswordService.Verify(dto.PasswordActual, usuario.Password))
                throw new BusinessException("La contraseña actual es incorrecta.", 400);

            if (dto.PasswordNueva == dto.PasswordActual)
                throw new BusinessException("La nueva contraseña no puede ser igual a la actual.", 400);

            usuario.Password = PasswordService.Hash(dto.PasswordNueva);

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(AutenticarUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new BusinessException("Usuario y contraseña obligatorios.", 400);

            var usuario = await _usuarioRepository.ObtenerPorUsernameAsync(dto.Username);

            if (usuario == null)
                throw new BusinessException("Usuario o contraseña incorrectos.", 401);

            if (!usuario.Estado)
                throw new BusinessException("Usuario inactivo.", 403);

            if (!PasswordService.Verify(dto.Password, usuario.Password))
                throw new BusinessException("Usuario o contraseña incorrectos.", 401);

            var token = _jwtHelper.GenerarToken(
                usuario.UsuarioId,
                usuario.Username,
                usuario.Rol?.Nombre ?? ""
            );

            return new AppSesionUsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                UsuarioNombre = $"{usuario.Nombre} {usuario.Apellidos}",
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre ?? "",
                Token = token
            };
        }

        private string GenerarCodigoUsuario(int usuarioId)
        {
            return $"USER-{usuarioId:D4}";
        }
    }
}