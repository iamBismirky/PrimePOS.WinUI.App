using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services
{
    public class PdfViewService
    {
        //public async Task MostrarPdfAsync(byte[] pdfBytes)
        //{
        //    string tempFile =
        //        Path.Combine(
        //            Path.GetTempPath(),
        //            $"etiqueta-{Guid.NewGuid()}.pdf");

        //    await File.WriteAllBytesAsync(
        //        tempFile,
        //        pdfBytes);

        //    System.Diagnostics.Process.Start(
        //        new System.Diagnostics.ProcessStartInfo
        //        {
        //            FileName = tempFile,
        //            UseShellExecute = true
        //        });
        //}
        private readonly MainWindow _mainWindow;

        public PdfViewService(
            MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public async Task MostrarPdfAsync(
            byte[] pdfBytes)
        {
            await _mainWindow
                .MostrarPdfAsync(pdfBytes);
        }
    }
}
