using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface ICajaRepository
    {
        void Actualizar(Caja caja);
        void Crear(Caja caja);
        void Eliminar(Caja caja);
        Task<Caja?> ObtenerPorNombreAsync(string nombre);
        Task GuardarCambiosAsync();
        Task<List<Caja>> ListarCajasAsync();
        Task<Caja?> ObtenerCajaPorIdAsync(int id);
        Task<bool> ExisteCajaIdAsync(int cajaId);
    }
}