using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Empresa;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models.Seguridad;

namespace PrimePOS.BLL.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IHostEnvironment _env;

    public EmpresaService(IEmpresaRepository empresaRepo, IHostEnvironment env)
    {
        _empresaRepo = empresaRepo;
        _env = env;
    }

    public async Task<int> CrearAsync(CrearEmpresaDto dto)
    {
        if (string.IsNullOrEmpty(dto.Nombre))
            throw new BusinessException("El nombre es obligatorio", StatusCodes.Status400BadRequest);


        var empresa = new Empresa
        {
            Nombre = dto.Nombre,
            RNC = dto.RNC,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            Activa = dto.Activa,
            FechaRegistro = DateTime.Now
        };

        _empresaRepo.Crear(empresa);
        await _empresaRepo.GuardarCambiosAsync();
        return empresa.EmpresaId;
    }

    public async Task SubirLogoAsync(int empresaId, IFormFile logo)
    {
        var empresa =
        await _empresaRepo.ObtenerPorIdAsync(empresaId);
        if (empresa == null)
            throw new BusinessException("La empresa no existe", StatusCodes.Status404NotFound);

        var carpeta = Path.Combine(
            _env.ContentRootPath,
            "wwwroot",
            "logos");

        Directory.CreateDirectory(carpeta);

        var extension =
            Path.GetExtension(logo.FileName);

        var nombreArchivo =
            $"empresa_{empresaId}{extension}";

        var ruta =
            Path.Combine(
                carpeta,
                nombreArchivo);

        using var stream =
            new FileStream(
                ruta,
                FileMode.Create);

        await logo.CopyToAsync(stream);

        empresa.LogoUrl =
            $"logos/{nombreArchivo}";

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
        empresa.Email = dto.Email;
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
            Email = empresa.Email,
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
            Email = e.Email,
            Direccion = e.Direccion,
            Activa = e.Activa,
            FechaRegistro = e.FechaRegistro
        }).ToList();
    }
}