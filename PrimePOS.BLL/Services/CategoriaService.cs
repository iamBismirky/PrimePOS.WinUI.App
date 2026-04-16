using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.Contracts.DTOs.Categoria;
using PrimePOS.DAL.Interfaces;
using PrimePOS.ENTITIES.Models;
using System.Data;

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
            throw new BusinessException("La categoría no puede estar vacía.", "REQUIRED");

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
            throw new BusinessException("Ya existe una categoría con este nombre.", "DUPLICATE");

        }
        else
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Estado = true
            };

            _categoriaRepository.Crear(categoria);
            await _categoriaRepository.GuardarCambiosAsync();
        }

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
        var dto = await _categoriaRepository.ObtenerPorIdAsync(categoriaId);
        if (dto == null) return null;

        return new CategoriaDto
        {
            CategoriaId = dto.CategoriaId,
            Nombre = dto.Nombre
        };

    }

    public async Task ActualizarCategoriaAsync(CategoriaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new BusinessException("La categoría no puede estar vacía.", "REQUIRED");

        //  Buscar por nombre primero
        var existente = await _categoriaRepository.ExisteCategoriaAsync(dto.Nombre);

        if (existente != null)
        {
            // 👉 Si existe pero está inactiva → reactivar
            if (existente.Estado == false)
            {
                existente.Estado = true;

                _categoriaRepository.Actualizar(existente);
                await _categoriaRepository.GuardarCambiosAsync();

            }

            // Si existe y está activa → error
            if (existente.CategoriaId != dto.CategoriaId)
            {
                throw new BusinessException("Ya existe una categoría con ese nombre.", "DUPLICATE");
            }
        }
        else
        {
            var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaId)
            ?? throw new BusinessException("Debe seleccionar una categoría", "REQUIRED");

            categoria.Nombre = dto.Nombre;

            _categoriaRepository.Actualizar(categoria);
            await _categoriaRepository.GuardarCambiosAsync();



        }

    }

    public async Task<bool> EliminarCategoriaAsync(int categoriaId)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(categoriaId);

        if (categoria == null)
            throw new BusinessException("Debe de seleccionar una categoria", "REQUIRED");

        _categoriaRepository.Eliminar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();
        return true;
    }
    public async Task DesactivarCategoriaAsync(int categoriaId)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(categoriaId);

        if (categoria == null)
            throw new BusinessException("Debe de seleccionar una categoria", "REQUIRED");

        categoria.Estado = false;
        _categoriaRepository.Actualizar(categoria);
        await _categoriaRepository.GuardarCambiosAsync();

    }
}

