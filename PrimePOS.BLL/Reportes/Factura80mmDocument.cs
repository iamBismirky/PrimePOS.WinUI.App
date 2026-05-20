using PrimePOS.Contracts.DTOs.Factura;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PrimePOS.BLL.Reportes;

public class Factura80mmDocument : IDocument
{
    private readonly FacturaDto _factura;

    public Factura80mmDocument(FacturaDto factura)
    {
        _factura = factura;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.ContinuousSize(80, Unit.Millimetre);
            page.Margin(5);

            page.DefaultTextStyle(x =>
                x.FontSize(9));

            page.Content().Column(column =>
            {
                column.Spacing(4);

                // HEADER
                column.Item()
                    .AlignCenter()
                    .Text("PRIMEPOS")
                    .Bold()
                    .FontSize(16);

                column.Item()
                    .AlignCenter()
                    .Text("Dirección de la tienda");

                column.Item()
                    .AlignCenter()
                    .Text("RNC");

                column.Item()
                    .AlignCenter()
                    .Text("Tel: 123-456-7890");

                column.Item()
                    .PaddingVertical(3)
                    .LineHorizontal(1);

                // INFO FACTURA
                column.Item().Text($"Fecha: {_factura.Fecha:dd/MM/yyyy HH:mm}");
                column.Item().Text($"Factura: {_factura.Numero}");

                column.Item().Text($"Turno: {_factura.Turno} | Cajero: {_factura.UsuarioNombre} ");


                column.Item().Text($"Metodo Pago: {_factura.MetodoPago}");

                column.Item().Text($"Cliente: {_factura.ClienteNombre}");

                column.Item()
                    .PaddingVertical(3)
                    .LineHorizontal(1);

                // ENCABEZADO PRODUCTOS
                column.Item().Row(row =>
                {
                    row.RelativeItem()
                        .Text("Producto")
                        .SemiBold();

                    row.ConstantItem(35)
                        .AlignCenter()
                        .Text("Cant")
                        .SemiBold();

                    row.ConstantItem(50)
                        .AlignRight()
                        .Text("Total")
                        .SemiBold();
                });

                column.Item().LineHorizontal(1);

                // PRODUCTOS
                foreach (var item in _factura.Detalles)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem()
                            .Text(item.Nombre);

                        row.ConstantItem(35)
                            .AlignCenter()
                            .Text(item.Cantidad.ToString());

                        row.ConstantItem(50)
                            .AlignRight()
                            .Text(item.Total.ToString("N2"));
                    });
                }

                column.Item()
                    .PaddingVertical(3)
                    .LineHorizontal(1);

                // TOTALES
                AgregarTotal(column, "Subtotal", _factura.Subtotal);
                AgregarTotal(column, "ITBIS", _factura.Impuesto);

                column.Item().Row(row =>
                {
                    row.RelativeItem()
                        .Text("TOTAL")
                        .Bold()
                        .FontSize(12);

                    row.ConstantItem(70)
                        .AlignRight()
                        .Text(_factura.Total.ToString("N2"))
                        .Bold()
                        .FontSize(12);
                });

                column.Item().Text($"Efectivo: {_factura.Efectivo:N2}");
                column.Item().Text($"Cambio: {_factura.Cambio:N2}");

                column.Item()
                    .PaddingVertical(3)
                    .LineHorizontal(1);

                // FOOTER
                column.Item()
                    .AlignCenter()
                    .Text("¡Gracias por su compra!")
                    .Italic();
            });
        });
    }

    private void AgregarTotal(
        ColumnDescriptor column,
        string label,
        decimal value)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem()
                .Text(label);

            row.ConstantItem(70)
                .AlignRight()
                .Text(value.ToString("N2"));
        });
    }
}