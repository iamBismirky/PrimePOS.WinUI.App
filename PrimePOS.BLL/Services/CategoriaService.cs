using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _categoriaRepository = repository;
    }

    public async Task CrearCategoriaAsync(CategoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("La categoría no puede estar vacía.", 400);

        var existe = await _categoriaRepository.ExisteCategoriaAsync(dto.Nombre);

        if (existe != null)
        {
            if (!existe.Estado)
            {
                existe.Estado = true;
                _categoriaRepository.Actualizar(existe);
                await _categoriaRepository.GuardarCambiosAsync();
                return;
            }

            throw new BusinessException("Ya existe una categoría con este nombre.", 400);
        }

        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Estado = true
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

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(categoriaId);

        if (categoria == null)
            throw new BusinessException("La categoría no existe.", 404);

        return new CategoriaDto
        {
            CategoriaId = categoria.CategoriaId,
            Nombre = categoria.Nombre,
            Estado = categoria.Estado
        };
    }

    public async Task ActualizarCategoriaAsync(CategoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("La categoría no puede estar vacía.", 400);

        var existente = await _categoriaRepository.ExisteCategoriaAsync(dto.Nombre);

        if (existente != null)
        {
            // Reactivar si está inactiva
            if (!existente.Estado)
            {
                existente.Estado = true;

                _categoriaRepository.Actualizar(existente);
                await _categoriaRepository.GuardarCambiosAsync();
                return;
            }

            // Si es otra categoría con el mismo nombre
            if (existente.CategoriaId != dto.CategoriaId)
                throw new BusinessException("Ya existe una categoría con ese nombre.", 400);
        }

        var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaId);

        if (categoria == null)
            throw new BusinessException("La categoría no existe.", 404);

        categoria.Nombre = dto.Nombre;

        _categoriaRepository.Actualizar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
    }

    public async Task EliminarCategoriaAsync(int categoriaId)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(categoriaId);

        if (categoria == null)
            throw new BusinessException("La categoría no existe.", 404);

        _categoriaRepository.Eliminar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
    }

    public async Task DesactivarCategoriaAsync(int categoriaId)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(categoriaId);

        if (categoria == null)
            throw new BusinessException("La categoría no existe.", 404);

        categoria.Estado = false;

        _categoriaRepository.Actualizar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
    }
}