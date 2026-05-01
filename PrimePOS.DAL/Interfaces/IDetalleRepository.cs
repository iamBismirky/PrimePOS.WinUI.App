using PrimePOS.ENTITIES.Models;

namespace PrimePOS.DAL.Interfaces
{
    public interface IDetalleRepository
    {
        void Agregar(VentaDetalle detalle);
    }
}