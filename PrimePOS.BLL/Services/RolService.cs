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
    //Crear un rol
    public async Task CrearRolAsync(CrearRolDto dto)
    {

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del rol es obligatorio", "REQUIRED");

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
            throw new BusinessException("Ya existe un rol con ese nombre", "DUPLICATE");


        }
        else
        {
            var rol = new Rol
            {
                Nombre = dto.Nombre,
                Estado = true
            };
            await _rolRepository.Crear(rol);
            await _rolRepository.GuardarCambiosAsync();

        }


    }
    //Actualizar rol
    public async Task<bool> ActualizarRolAsync(ActualizarRolDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("El nombre del rol es obligario", "REQUIRED");

        var rol = await _rolRepository.ObtenerPorIdAsync(dto.RolId);

        if (rol == null)
            throw new BusinessException("Debe seleccionar un Rol", "REQUIRED");

        if (dto.Nombre == rol.Nombre && dto.Estado == rol.Estado)
        {
            throw new BusinessException("Ya existe un rol con este nombre", "DUPLICATE");
        }

        rol.Nombre = dto.Nombre;
        rol.Estado = true;

        await _rolRepository.Actualizar(rol);
        await _rolRepository.GuardarCambiosAsync();
        return true;
    }


    //Eliminar rol
    public async Task<bool> EliminarRolAsync(EliminarRolDto dto)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(dto.RolId);

        if (rol == null)
            throw new BusinessException("Debe seleccionar un Rol", "REQUIRED");

        await _rolRepository.Eliminar(rol);
        await _rolRepository.GuardarCambiosAsync();
        return true;
    }
    public async Task DesactivarRolAsync(int rolId)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(rolId);

        if (rol == null)
            throw new BusinessException("Debe seleccionar un Rol", "REQUIRED");

        rol.Estado = false;
        await _rolRepository.Actualizar(rol);
        await _rolRepository.GuardarCambiosAsync();

    }
    //Obtener rol por id
    public async Task<RolDto?> ObtenerRolPorIdAsync(int id)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(id);

        if (rol == null) return null;

        return new RolDto
        {
            RolId = rol.RolId,
            Nombre = rol.Nombre,
            Estado = rol.Estado,
        };



    }
    //Listar roles
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
