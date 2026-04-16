using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface ICategoriaRepository
    {
        void Actualizar(Categoria categoria);
        void Crear(Categoria categoria);
        void Eliminar(Categoria categoria);
        Task<Categoria?> ExisteCategoriaAsync(string nombre);
        Task GuardarCambiosAsync();
        Task<List<Categoria>> ListarCategoriaAsync();
        Task<Categoria?> ObtenerPorIdAsync(int categoriaId);
    }
}