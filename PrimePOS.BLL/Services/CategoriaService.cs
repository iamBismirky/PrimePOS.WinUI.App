using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;

namespace PrimePOS.BLL.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
    {
        _repository = repository;
    }

    public void AgregarCategoria(string nombre, bool estado)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("La categoría no puede estar vacía.");

        if (_repository.ExisteNombre(nombre))
            throw new Exception("Ya existe una categoría con ese nombre.");

        var categoria = new Categoria
        {
            Nombre = nombre,
            Estado = estado
        };

        _repository.Agregar(categoria);
    }

    public List<Categoria> ListarCategorias()
    {
        return _repository.Listar();
    }

    public void ActualizarCategoria(int id, string nombre, bool estado)
    {
        var categoria = _repository.ObtenerPorId(id)
            ?? throw new Exception("Categoría no encontrada.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("La categoría no puede estar vacía.");

        if (_repository.ExisteNombre(nombre, id))
            throw new Exception("Ya existe una categoría con ese nombre.");

        categoria.Nombre = nombre;
        categoria.Estado = estado;

        _repository.Actualizar(categoria);
    }

    public void EliminarCategoria(int id)
    {
        var categoria = _repository.ObtenerPorId(id)
            ?? throw new Exception("Categoría no encontrada.");

        _repository.Eliminar(categoria);
    }
}

