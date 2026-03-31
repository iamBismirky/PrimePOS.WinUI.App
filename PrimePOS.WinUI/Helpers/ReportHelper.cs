using FastReport;
using FastReport.Export.PdfSimple;
using PrimePOS.BLL.DTOs.Factura;

namespace PrimePOS.WinUI.Helpers
{
    public static class ReportHelper
    {
        public static void MostrarFactura(FacturaDto factura)
        {
            using var report = new Report();

            report.Load("Reportes/Factura8cm.frx");

            report.RegisterData(new[] { factura }, "Factura");
            report.RegisterData(factura.Detalles, "Detalles");

            report.GetDataSource("Factura").Enabled = true;
            report.GetDataSource("Detalles").Enabled = true;

            report.Prepare();
            var export = new PDFSimpleExport();
            report.Export(export, "factura.pdf");
        }
    }
}
