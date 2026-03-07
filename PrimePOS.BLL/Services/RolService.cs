using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System.Diagnostics;

namespace PrimePOS.BLL.Services;



public class RolService
{
    private readonly RolRepository _repository;
    public RolService(RolRepository repository)
    {
        _repository = repository;
    }
    //Crear un rol
    public async Task CrearRolAsync(CrearRolDto dto)
    {
        var rol = new Rol
        {
            Descripcion = dto.Descripcion,
            Estado = dto.Estado
        };

        await _repository.CrearRolAsync(rol);
        await _repository.GuardarCambiosAsync();
    }
    //Actualizar rol
    public async Task<bool> ActualizarRolAsync(ActualizarRolDto dto)
    {
        var rol = await _repository.ObtenerPorIdAsync(dto.RolId);

        if (rol == null)
            throw new Exception("Rol no encontrado.");


        rol.Descripcion = dto.Descripcion;
        rol.Estado = dto.Estado;

        await _repository.ActualizarRolAsync(rol);
        await _repository.GuardarCambiosAsync();
        return true;
    }
    //Elimninar rol
    public async Task<bool> EliminarRolAsync(EliminarRolDto dto)
    {
        var rol = await _repository.ObtenerPorIdAsync(dto.RolId);

        if (rol == null)
            throw new Exception("Rol no encontrado");

        await _repository.EliminarRolAsync(rol);
        await _repository.GuardarCambiosAsync();
        return true;
    }
    //Obtener rol por id
    public async Task<RolDto?> ObtenerRolPorIdAsync(int id)
    {
        var rol = await _repository.ObtenerPorIdAsync(id);

        if (rol == null) return null;

        return new RolDto
        {
            RolId = rol.RolId,
            Descripcion = rol.Descripcion,
            Estado = rol.Estado,
        };



    }
    //Listar roles
    public async Task<List<ListaRolesDto>> ListarRolesAsync()
    {
        var roles = await _repository.ListarRolesAsync();

        return roles.Select(r => new ListaRolesDto
        {
            RolId = r.RolId,
            Descripcion = r.Descripcion,
            Estado = r.Estado
        }).ToList();
    }

}
