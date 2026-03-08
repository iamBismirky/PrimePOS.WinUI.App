using PrimePOS.BLL.DTOs.Categoria;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System.Data;

namespace PrimePOS.BLL.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task CrearCategoriaAsync(CategoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("La categoría no puede estar vacía.");

        var existe = await _repository.ExisteCategoriaAsync(dto.Nombre);

        if (existe)
            throw new Exception("Ya existe una categoría con este nombre.");

        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Estado = dto.Estado
        };

        await _repository.CrearCategoriaAsync(categoria);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<List<CategoriaDto>> ListarCategoriasAsync()
    {
        var categorias = await _repository.ListarCategoriaAsync();
        return categorias.Select(c => new CategoriaDto
        {
            CategoriaId = c.CategoriaId,
            Nombre = c.Nombre,
            Estado = c.Estado
        }).ToList();
    }

    public async Task<bool> ActualizarCategoriaAsync(CategoriaDto dto)
    {
        var categoria = await _repository.ObtenerPorIdAsync(dto.CategoriaId)
            ?? throw new Exception("Categoría no encontrada.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("La categoría no puede estar vacía.");

        var existe = await _repository.ExisteCategoriaAsync(dto.Nombre);
        if (existe)
            throw new Exception("Ya existe una categoría con ese nombre.");

        categoria.Nombre = dto.Nombre;
        categoria.Estado = dto.Estado;

        await _repository.ActualizarCategoriaAsync(categoria);
        await _repository.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarCategoriaAsync(CategoriaDto dto)
    {
        var categoria = await _repository.ObtenerPorIdAsync(dto.CategoriaId);

        if (categoria == null)
            throw new Exception("Categoría no encontrada.");

        await _repository.EliminarCategoriaAsync(categoria);
        await _repository.GuardarCambiosAsync();
        return true;
    }
}

