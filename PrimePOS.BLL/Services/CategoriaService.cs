using PrimePOS.BLL.DTOs.Categoria;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System.Data;

namespace PrimePOS.BLL.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _categoriaRepository;

    public CategoriaService(CategoriaRepository repository)
    {
        _categoriaRepository = repository;
    }

    public async Task CrearCategoriaAsync(CategoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("La categoría no puede estar vacía.");

        var existe = await _categoriaRepository.ExisteCategoriaAsync(dto.Nombre);

        if (existe)
            throw new Exception("Ya existe una categoría con este nombre.");

        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Estado = dto.Estado
        };

        _categoriaRepository.Crear(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
    }

    public async Task<List<CategoriaDto>> ListarCategoriasAsync()
    {
        var categorias = await _categoriaRepository.ListarCategoriaAsync();
        return categorias.Select(c => new CategoriaDto
        {
            CategoriaId = c.CategoriaId,
            Nombre = c.Nombre,
            Estado = c.Estado
        }).ToList();
    }

    public async Task<bool> ActualizarCategoriaAsync(CategoriaDto dto)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaId)
            ?? throw new Exception("Debe seleccionar una categoria");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("La categoría no puede estar vacía.");

        var existe = await _categoriaRepository.ExisteCategoriaAsync(dto.Nombre);
        if (existe)
            throw new Exception("Ya existe una categoría con ese nombre.");

        categoria.Nombre = dto.Nombre;
        categoria.Estado = dto.Estado;

        _categoriaRepository.Actualizar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarCategoriaAsync(CategoriaDto dto)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaId);

        if (categoria == null)
            throw new Exception("Debe de seleccionar una categoria");

        _categoriaRepository.Eliminar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
        return true;
    }
}

