using PrimePOS.Contracts.DTOs.Categoria;

namespace PrimePOS.BLL.Interfaces
{
    public interface ICategoriaService
    {
        Task ActualizarCategoriaAsync(CategoriaDto dto);
        Task CrearCategoriaAsync(CategoriaDto dto);
        Task<bool> EliminarCategoriaAsync(int categoriaId);
        Task DesactivarCategoriaAsync(int categoriaId);
        Task<List<CategoriaDto>> ListarCategoriasAsync();
        Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId);
    }
}