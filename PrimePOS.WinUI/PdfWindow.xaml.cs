using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PdfWindow : Window
    {
        public PdfWindow()
        {
            InitializeComponent();


        }

        public async Task LoadUrlAsync(string url)
        {
            await PdfViewer.EnsureCoreWebView2Async();

            PdfViewer.Source = new Uri(url);
        }

        public async Task LoadBytesAsync(byte[] pdfBytes)
        {
            await PdfViewer.EnsureCoreWebView2Async();

            var filePath = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.pdf");

            await File.WriteAllBytesAsync(filePath, pdfBytes);

            PdfViewer.Source = new Uri(filePath);
        }
    }
}
