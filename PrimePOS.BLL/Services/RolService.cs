using PrimePOS.BLL.DTOs.Rol;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

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
        await Validar(dto);

        var rol = new Rol
        {
            Nombre = dto.Nombre,
            Estado = dto.Estado
        };

        await _repository.CrearRolAsync(rol);
        await _repository.GuardarCambiosAsync();
    }
    //Actualizar rol
    public async Task<bool> ActualizarRolAsync(ActualizarRolDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new Exception("El nombre del rol es obligario");
        }
        var rol = await _repository.ObtenerPorIdAsync(dto.RolId);

        if (rol == null)
            throw new Exception("Rol no encontrado.");

        if (dto.Nombre == rol.Nombre)
        {
            throw new Exception("Ya existe un rol con este nombree");
        }

        rol.Nombre = dto.Nombre;
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
            Nombre = rol.Nombre,
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
            Nombre = r.Nombre,
            Estado = r.Estado
        }).ToList();
    }
    private async Task Validar(CrearRolDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("El nombre del rol es obligatorio");

        var existe = await _repository.ExisteRol(dto.Nombre);

        if (existe)
            throw new Exception("Ya existe un rol con ese nombre");
    }
}
