using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services
{
    public class PdfViewService
    {

        private readonly MainWindow _mainWindow;

        public PdfViewService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public Task MostrarFacturaAsync(string url)
        {
            _mainWindow.ShowPdf(url);
            return Task.CompletedTask;
        }

        public Task MostrarEtiquetaPdfAsync(byte[] pdfBytes)
        {
            _mainWindow.ShowPdfBytes(pdfBytes);
            return Task.CompletedTask;
        }

        public void Cerrar()
        {
            _mainWindow.HidePdf();
        }
    }
}
