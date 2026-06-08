using Microsoft.AspNetCore.Http;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Empresa;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Seguridad;

namespace PrimePOS.BLL.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _empresaRepo;

    public EmpresaService(IEmpresaRepository empresaRepo)
    {
        _empresaRepo = empresaRepo;
    }

    public async Task CrearAsync(CrearEmpresaDto dto)
    {
        if (string.IsNullOrEmpty(dto.Nombre))
            throw new BusinessException("El nombre es obligatorio", StatusCodes.Status400BadRequest);


        var empresa = new Empresa
        {
            Nombre = dto.Nombre,
            RNC = dto.RNC,
            LogoUrl = dto.LogoUrl,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            Direccion = dto.Direccion,
            Activa = dto.Activa,
            FechaRegistro = DateTime.Now
        };

        _empresaRepo.Crear(empresa);
        await _empresaRepo.GuardarCambiosAsync();
    }
    public async Task ActualizarAsync(int id, ActualizarEmpresaDto dto)
    {
        var empresa = await _empresaRepo.ObtenerPorIdAsync(id);
        if (empresa == null)
            throw new BusinessException("La empresa no existe", StatusCodes.Status404NotFound);
        if (string.IsNullOrEmpty(dto.Nombre))
            throw new BusinessException("El nombre es obligatorio", StatusCodes.Status400BadRequest);
        empresa.Nombre = dto.Nombre;
        empresa.RNC = dto.RNC;
        empresa.LogoUrl = dto.LogoUrl;
        empresa.Telefono = dto.Telefono;
        empresa.Correo = dto.Correo;
        empresa.Direccion = dto.Direccion;
        empresa.Activa = dto.Activa;
        _empresaRepo.Actualizar(empresa);
        await _empresaRepo.GuardarCambiosAsync();
    }
    public async Task EliminarAsync(int empresaId)
    {
        var empresa = await _empresaRepo.ObtenerPorIdAsync(empresaId);
        if (empresa == null)
            throw new BusinessException("La empresa no existe", StatusCodes.Status404NotFound);
        _empresaRepo.Eliminar(empresa);
        await _empresaRepo.GuardarCambiosAsync();
    }
    public async Task DesactivarAsync(int empresaId)
    {
        var empresa = await _empresaRepo.ObtenerPorIdAsync(empresaId);
        if (empresa == null)
            throw new BusinessException("La empresa no existe", StatusCodes.Status404NotFound);
        empresa.Activa = false;
        _empresaRepo.Actualizar(empresa);
        await _empresaRepo.GuardarCambiosAsync();
    }
    public async Task<EmpresaDto?> ObtenerPorIdAsync(int empresaId)
    {
        var empresa = await _empresaRepo.ObtenerPorIdAsync(empresaId);
        if (empresa == null)
            throw new BusinessException("La empresa no existe", StatusCodes.Status404NotFound);

        return new EmpresaDto
        {
            EmpresaId = empresa.EmpresaId,
            Nombre = empresa.Nombre,
            RNC = empresa.RNC,
            LogoUrl = empresa.LogoUrl,
            Telefono = empresa.Telefono,
            Correo = empresa.Correo,
            Direccion = empresa.Direccion,
            Activa = empresa.Activa,
            FechaRegistro = empresa.FechaRegistro
        };
    }
    public async Task<List<EmpresaDto>> ObtenerTodosAsync()
    {
        var empresas = await _empresaRepo.ObtenerTodosAsync();
        return empresas.Select(e => new EmpresaDto
        {
            EmpresaId = e.EmpresaId,
            Nombre = e.Nombre,
            RNC = e.RNC,
            LogoUrl = e.LogoUrl,
            Telefono = e.Telefono,
            Correo = e.Correo,
            Direccion = e.Direccion,
            Activa = e.Activa,
            FechaRegistro = e.FechaRegistro
        }).ToList();
    }
}