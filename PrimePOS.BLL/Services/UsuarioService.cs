using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PrimePOS.BLL.DTOs.Usuario;
using PrimePOS.BLL.Security;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.BLL.Services
{
    public class UsuarioService 
    {
        private readonly UsuarioRepository _repository;

        public UsuarioService(UsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task AgregarUsuarioAsync(CrearUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Apellidos) ||
                string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Todos los campos son obligatorios.");

            if (dto.RolId == 0)
                throw new Exception("Seleccione un rol.");

            if (await _repository.ExisteUsernameAsync(dto.Username))
                throw new Exception("Ya existe un usuario con ese username.");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Username = dto.Username,
                Password = PasswordService.Hash(dto.Password),
                Estado = dto.Estado,
                FechaCreacion = DateTime.Now,
                RolId = dto.RolId
            };

            await _repository.CrearUsuarioAsync(usuario);
            await _repository.GuardarCambiosAsync();
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            return await _repository.ListarUsuariosAsync();
        }

        public async Task<bool> ActualizarUsuarioAsync(ActualizarUsuarioDto dto)
        {
            var usuario = await _repository.ObtenerUsuarioPorIdAsync(dto.UsuarioId)
                ?? throw new Exception("Usuario no encontrado.");

            if (await _repository.ExisteUsernameAsync(dto.Username, dto.UsuarioId))
                throw new Exception("Ya existe un usuario con ese username.");

            usuario.Nombre = dto.Nombre;
            usuario.Apellidos = dto.Apellidos;
            usuario.Username = dto.Username;
            usuario.Estado = dto.Estado;
            usuario.RolId = dto.RolId;

            await _repository.ActualizarUsuarioAsync(usuario);
            await _repository.GuardarCambiosAsync();
            return true;
        }

        public async Task<bool> EliminarUsuarioAsync(EliminarUsuarioDto dto)
        {
            var usuario = await _repository.ObtenerUsuarioPorIdAsync(dto.UsuarioId)
                ?? throw new Exception("Usuario no encontrado.");

            await _repository.EliminarUsuarioAsync(usuario);
            await _repository.GuardarCambiosAsync();
            return true;
        }

        public async Task CambiarEstadoAsync(int id, bool nuevoEstado)
        {
            var usuario = await _repository.ObtenerUsuarioPorIdAsync(id)
                ?? throw new Exception("Usuario no encontrado.");

            usuario.Estado = nuevoEstado;

            await _repository.ActualizarUsuarioAsync(usuario);
            await _repository.GuardarCambiosAsync();
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

            var usuario = await _repository.ObtenerUsuarioPorIdAsync(id)
                ?? throw new Exception("Usuario no encontrado.");

            bool esValida = PasswordService.Verify(contraseñaActual, usuario.Password);

            if (!esValida)
                throw new Exception("La contraseña actual es incorrecta.");

            if (PasswordService.Verify(contraseñaNueva, usuario.Password))
                throw new Exception("La nueva contraseña no puede ser igual a la actual.");

            usuario.Password = PasswordService.Hash(contraseñaNueva);

            await _repository.ActualizarUsuarioAsync(usuario);
            await _repository.GuardarCambiosAsync();
        }

        public async Task<Usuario> AutenticarAsync(string username, string clave)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(clave))
                throw new Exception("Usuario y clave obligatorios.");

            var usuario = await _repository.ObtenerPorUsernameAsync(username)
                ?? throw new Exception("Usuario o clave incorrectos.");

            if (!usuario.Estado)
                throw new Exception("Usuario inactivo.");

            bool esValida = PasswordService.Verify(clave, usuario.Password);

            if (!esValida)
                throw new Exception("Usuario o clave incorrectos.");

            return usuario;
        }

        public async Task<List<Rol>> ListarRolesAsync()
        {
            return await _repository.ListarRolesAsync();
        }
    }
}


