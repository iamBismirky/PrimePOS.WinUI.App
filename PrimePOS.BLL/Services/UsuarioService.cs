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

            var usuario = await _usuarioRepository.ObtenerPorId(dto.UsuarioId)
                ?? throw new BusinessException("Usuario no encontrado. Seleccione uno.", "NO_FOUND");

            if (await _usuarioRepository.ExisteUsernameAsync(dto.Username, dto.UsuarioId))
                throw new BusinessException("Ya existe un usuario con ese username.", "DUPLICATE");

            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Username = dto.Username;
            usuario.Estado = dto.Estado;
            usuario.RolId = dto.RolId;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();

        }

        public async Task<bool> EliminarUsuarioAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(usuarioId)
                ?? throw new BusinessException("Usuario no encontrado. Seleccione uno.", "NO_FOUND");

            _usuarioRepository.Eliminar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
            return true;
        }
        public async Task<bool> DesactivarUsuarioAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(usuarioId)
                ?? throw new BusinessException("Usuario no encontrado. Seleccione uno.", "NO_FOUND");

            usuario.Estado = false;
            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
            return true;
        }

        public async Task CambiarEstadoAsync(int id, bool nuevoEstado)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(id)
                ?? throw new BusinessException("Usuario no encontrado.", "NO_FOUND");

            usuario.Estado = nuevoEstado;

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }
        public async Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(id);

            if (usuario == null) return null;

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,

                Nombre = usuario.Nombre,
                Codigo = usuario.Codigo,
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
        public async Task CambiarContraseñaAsync(CambiarContraseñaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ContraseñaActual))
                throw new BusinessException("La contraseña actual es obligatoria.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.ContraseñaNueva))
                throw new BusinessException("La nueva contraseña es obligatoria.", "REQUIRED");

            if (string.IsNullOrWhiteSpace(dto.Confirmar))
                throw new BusinessException("Confirme la contraseña.", "REQUIRED");

            if (dto.ContraseñaNueva != dto.Confirmar)
                throw new BusinessException("La confirmación no coincide con la nueva contraseña.", "INVALID");

            var usuario = await _usuarioRepository.ObtenerPorId(dto.UsuarioId)
                ?? throw new BusinessException("Usuario no encontrado.", "NO_FOUND");

            bool esValida = PasswordService.Verify(dto.ContraseñaActual, usuario.Password);

            if (!esValida)
                throw new BusinessException("La contraseña actual es incorrecta.", "INVALID");

            if (PasswordService.Verify(dto.ContraseñaNueva, usuario.Password))
                throw new BusinessException("La nueva contraseña no puede ser igual a la actual.", "INVALID");

            usuario.Password = PasswordService.Hash(dto.ContraseñaNueva);

            _usuarioRepository.Actualizar(usuario);
            await _usuarioRepository.GuardarCambiosAsync();
        }

        public async Task<AppSesionUsuarioDto> AutenticarUsuarioAsync(AutenticarUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Usuario y clave obligatorios.");

            var usuario = await _usuarioRepository.ObtenerPorUsernameAsync(dto.Username)
                ?? throw new BusinessException("Usuario o contraseña incorrectos.", "INVALID");

            if (!usuario.Estado)
                throw new BusinessException("Usuario inactivo.", "INACTIVE");

            bool esValida = PasswordService.Verify(dto.Password, usuario.Password);

            if (!esValida)
                throw new BusinessException("Usuario o contraseña incorrectos.", "INVALID");

            // 🔐 JWT
            var token = _jwtHelper.GenerarToken(
                usuario.UsuarioId,
                usuario.Username,
                usuario.Rol?.Nombre ?? ""
            );

            return new AppSesionUsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                UsuarioNombre = usuario.Nombre + " " + usuario.Apellidos,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre ?? "",
                Token = token

            };
        }

        private string GenerarCodigoUsuario(int UsuarioId)
        {
            return $"USER-{UsuarioId:D4}";
        }
    }
}


