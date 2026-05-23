using PrimePOS.ENTITIES.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PrimePOS.BLL.Reportes;

public class EtiquetaProductoDocument : IDocument
{
    private readonly Producto _producto;
    private readonly byte[] _barcodeBytes;

    public EtiquetaProductoDocument(
        Producto producto,
        byte[] barcodeBytes)
    {
        _producto = producto;
        _barcodeBytes = barcodeBytes;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(200, 125);

            page.Margin(10);

            page.Content().Column(column =>
            {
                column.Spacing(5);

                column.Item()
                    .Text(_producto.Nombre)
                    .FontSize(12)
                    .Bold();

                column.Item()
                    .Text($"RD$ {_producto.PrecioMinorista:N2}");

                column.Item()
                    .Image(_barcodeBytes);

                column.Item()
                    .AlignCenter()
                    .Text(_producto.Codigo)
                    .FontSize(10);
            });
        });
    }
}