using PrimePOS.ENTITIES.Models.Seguridad;

namespace PrimePOS.DAL.Interfaces
{
    public interface IEmpresaRepository
    {
        void Actualizar(Empresa empresa);
        void Crear(Empresa empresa);
        void Eliminar(Empresa empresa);
        Task GuardarCambiosAsync();
        Task<Empresa?> ObtenerPorIdAsync(int id);
        Task<List<Empresa>> ObtenerTodosAsync();
    }
}