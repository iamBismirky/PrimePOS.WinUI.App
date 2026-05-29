using System;
using System.IO;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services
{
    public class PdfViewService
    {
        // 🔥 VER FACTURA EN WEBVIEW2
        public async Task MostrarFacturaAsync(string url)
        {
            var window = new PdfWindow();

            window.Activate(); // abrir primero (no bloquea UI)

            await window.LoadUrlAsync(url); // luego cargar async
        }

        // 🔥 VER PDF DESDE BYTES
        public async Task MostrarEtiquetaPdfAsync(byte[] pdfBytes)
        {
            var window = new PdfWindow();

            window.Activate();

            await window.LoadBytesAsync(pdfBytes);
        }

        // 🔥 DESCARGAR PDF LOCAL
        public async Task<string> DescargarPdfAsync(byte[] pdfBytes, string fileName)
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "PrimePOS",
                "Facturas");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, fileName);

            await File.WriteAllBytesAsync(path, pdfBytes); // 🔥 IMPORTANTE

            return path;
        }
    }
}