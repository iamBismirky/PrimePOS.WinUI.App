using PrimePOS.ENTITIES.Models.Ventas;

namespace PrimePOS.DAL.Interfaces
{
    public interface IDetalleRepository
    {
        void Agregar(VentaDetalle detalle);
    }
}