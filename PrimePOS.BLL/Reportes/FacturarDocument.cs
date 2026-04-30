using PrimePOS.Contracts.DTOs.Factura;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PrimePOS.BLL.Reportes
{


    public class FacturaDocument : IDocument
    {
        private readonly FacturaDto _dto;

        public FacturaDocument(FacturaDto dto)
        {
            _dto = dto;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Content().Column(col =>
                {
                    col.Item().AlignCenter().Text("MI NEGOCIO").Bold().FontSize(18);

                    col.Item().Text($"Factura: {_dto.Numero}");
                    col.Item().Text($"Fecha: {_dto.Fecha:dd/MM/yyyy HH:mm}");

                    col.Item().LineHorizontal(1);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(60);
                        });

                        foreach (var item in _dto.Detalles)
                        {
                            table.Cell().Text(item.Nombre);
                            table.Cell().AlignRight().Text(item.Cantidad.ToString());
                            table.Cell().AlignRight().Text(item.Total.ToString("N2"));
                        }
                    });

                    col.Item().LineHorizontal(1);

                    col.Item().AlignRight().Text($"TOTAL: {_dto.Total:N2}").Bold();

                    col.Item().AlignCenter().Text("Gracias por su compra");
                });
            });
        }
    }
}
