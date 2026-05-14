namespace PrimePOS.BLL.Interfaces
{
    public interface IEtiquetaService
    {
        Task<byte[]> GenerarEtiquetaProductoAsync(int productoId);
    }
}