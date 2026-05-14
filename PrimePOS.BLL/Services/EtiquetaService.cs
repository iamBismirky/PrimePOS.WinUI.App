using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Reportes;
using PrimePOS.DAL.Interfaces;
using QuestPDF.Fluent;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp.Rendering;

namespace PrimePOS.BLL.Services;

public class EtiquetaService : IEtiquetaService
{
    private readonly IProductoRepository _repository;

    public EtiquetaService(
        IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> GenerarEtiquetaProductoAsync(
        int productoId)
    {
        var producto =
            await _repository.ObtenerPorIdAsync(productoId);

        if (producto == null)
            throw new BusinessException("Producto no encontrado", 404);

        byte[] barcodeBytes =
            GenerarBarcode(producto.Codigo);

        var report =
            new EtiquetaProductoDocument(
                producto,
                barcodeBytes);

        return report.GeneratePdf();
    }

    private byte[] GenerarBarcode(string codigo)
    {
        var writer = new BarcodeWriter<SKBitmap>
        {
            Format = BarcodeFormat.CODE_128,

            Options = new EncodingOptions
            {
                Width = 300,
                Height = 80,
                Margin = 2,
                PureBarcode = false
            },

            Renderer = new SKBitmapRenderer()
        };

        using var bitmap = writer.Write(codigo);

        using var image =
            SKImage.FromBitmap(bitmap);

        using var data =
            image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }
}