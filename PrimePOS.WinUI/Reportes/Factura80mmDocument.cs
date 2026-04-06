using PrimePOS.BLL.DTOs.Factura;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
namespace PrimePOS.WinUI.Reportes
{


    namespace PrimePOS.WinUI.Reportes
    {
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
                    page.Size(width: 200, height: 2000);
                    page.Margin(5); // Márgenes pequeños para térmica
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Column(stack =>
                    {
                        // Encabezado
                        stack.Item().Text("PRIME POS").Bold().AlignCenter();
                        stack.Item().Text("Dirección de la tienda").AlignCenter();
                        stack.Item().Text("Tel: 123-456-7890").AlignCenter();
                        stack.Item().LineHorizontal(1);

                        // Info venta
                        stack.Item().Text($"Fecha: {_factura.Fecha:dd/MM/yyyy HH:mm}");
                        stack.Item().Text($"Turno: {_factura.Turno}   Cajero: {_factura.UsuarioNombre}");
                        stack.Item().Text($"Cliente: {_factura.ClienteNombre}");
                        stack.Item().LineHorizontal(1);

                        // Items
                        stack.Item().Text("Producto           Cant  Total").SemiBold();
                        stack.Item().LineHorizontal(1);

                        foreach (var item in _factura.Detalles)
                        {
                            var nombre = item.Nombre.Length > 16 ? item.Nombre.Substring(0, 16) : item.Nombre.PadRight(16);
                            var cantidad = item.Cantidad.ToString().PadLeft(4);
                            var total = item.Total.ToString("0.00").PadLeft(6);

                            stack.Item().Text($"{nombre}{cantidad}{total}");
                        }

                        stack.Item().LineHorizontal(1);

                        // Totales
                        stack.Item().Text($"Subtotal: {_factura.Subtotal:0.00}");
                        stack.Item().Text($"Impuesto (18%): {_factura.Impuesto:0.00}");
                        stack.Item().Text($"Total: {_factura.Total:0.00}");
                        stack.Item().Text($"Efectivo: {_factura.Efectivo:0.00}");
                        stack.Item().Text($"Cambio: {_factura.Cambio:0.00}");
                        stack.Item().LineHorizontal(1);

                        // Mensaje final
                        stack.Item().Text("¡Gracias por su compra!").AlignCenter();
                    });
                });
            }
        }
    }
}
