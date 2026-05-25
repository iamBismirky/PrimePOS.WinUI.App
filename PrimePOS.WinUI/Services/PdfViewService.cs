using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services
{
    public class PdfViewService
    {
        public async Task MostrarFacturaAsync(string url)
        {
            var window = new PdfWindow();

            await window.LoadUrlAsync(url);

            window.Activate();
        }

        public async Task MostrarEtiquetaPdfAsync(byte[] pdfBytes)
        {
            var window = new PdfWindow();

            await window.LoadBytesAsync(pdfBytes);

            window.Activate();
        }
    }
}
