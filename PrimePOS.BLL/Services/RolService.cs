using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;

    public RolService(IRolRepository repository)
    {
        _rolRepository = repository;
    }

    // Crear
    public async Task CrearRolAsync(CrearRolDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del rol es obligatorio.", 400);

        var existe = await _rolRepository.ExisteRolAsync(dto.Nombre);

        if (existe != null)
        {
            if (!existe.Estado)
            {
                existe.Estado = true;
                await _rolRepository.Actualizar(existe);
                await _rolRepository.GuardarCambiosAsync();
                return;
            }

            throw new BusinessException("Ya existe un rol con ese nombre.", 400);
        }

        var rol = new Rol
        {
            Nombre = dto.Nombre,
            Estado = true
        };

        await _rolRepository.Crear(rol);
        await _rolRepository.GuardarCambiosAsync();
    }

    // Actualizar
    public async Task ActualizarRolAsync(ActualizarRolDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del rol es obligatorio.", 400);

        var rol = await _rolRepository.ObtenerPorIdAsync(dto.RolId);

        if (rol == null)
            throw new BusinessException("El rol no existe.", 404);

        var existe = await _rolRepository.ExisteRolAsync(dto.Nombre);

        if (existe != null && existe.RolId != dto.RolId)
        {
            if (!existe.Estado)
            {
                existe.Estado = true;
                await _rolRepository.Actualizar(existe);
                await _rolRepository.GuardarCambiosAsync();
                return;
            }

            throw new BusinessException("Ya existe un rol con ese nombre.", 400);
        }

        rol.Nombre = dto.Nombre;
        rol.Estado = dto.Estado;

        await _rolRepository.Actualizar(rol);
        await _rolRepository.GuardarCambiosAsync();
    }

    // Eliminar
    public async Task EliminarRolAsync(int rolId)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(rolId);

        if (rol == null)
            throw new BusinessException("El rol no existe.", 404);

        await _rolRepository.Eliminar(rol);
        await _rolRepository.GuardarCambiosAsync();
    }

    // Desactivar
    public async Task DesactivarRolAsync(int rolId)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(rolId);

        if (rol == null)
            throw new BusinessException("El rol no existe.", 404);

        rol.Estado = false;

        await _rolRepository.Actualizar(rol);
        await _rolRepository.GuardarCambiosAsync();
    }

    // Obtener por Id
    public async Task<RolDto?> ObtenerRolPorIdAsync(int id)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(id);

        if (rol == null)
            throw new BusinessException("El rol no existe.", 404);

        return new RolDto
        {
            RolId = rol.RolId,
            Nombre = rol.Nombre,
            Estado = rol.Estado,
        };
    }

    // Listar
    public async Task<List<ListaRolesDto>> ListarRolesAsync()
    {
        var roles = await _rolRepository.ListarRolesAsync();

        return roles.Select(r => new ListaRolesDto
        {
            RolId = r.RolId,
            Nombre = r.Nombre,
            Estado = r.Estado
        }).ToList();
    }
}