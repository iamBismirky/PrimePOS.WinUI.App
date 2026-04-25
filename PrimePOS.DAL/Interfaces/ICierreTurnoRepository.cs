using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface ICierreTurnoRepository
    {
        void Actualizar(CierreTurno cierreTurn);
        void Crear(CierreTurno cierreTurno);
        Task GuardarCambiosAsync();
    }
}