using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PrimePOS.BLL.Exceptions;
using PrimePOS.BLL.Interfaces;
using PrimePOS.BLL.Reportes;
using PrimePOS.Contracts.DTOs.Factura;
using QuestPDF.Fluent;

namespace PrimePOS.BLL.Services
{
    public class PdfService : IPdfService
    {
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PdfService(IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        // 🔥 GENERAR PDF
        public string GenerateFacturaPdf(FacturaDto factura)
        {
            var folder = GetFacturaFolder();

            var fileName = $"Factura-{factura.Numero}.pdf";
            var path = Path.Combine(folder, fileName);

            var document = new Factura80mmDocument(factura);
            document.GeneratePdf(path);

            return fileName;
        }

        // 🔥 OBTENER RUTA FÍSICA
        public string GetFilePath(string fileName)
        {
            var folder = GetFacturaFolder();
            var path = Path.Combine(folder, fileName);

            if (!File.Exists(path))
                throw new BusinessException("PDF no encontrado", 404);

            return path;
        }

        // 🔥 CARPETA FACTURAS (DIRECTO, SIN SERVICE EXTRA)
        private string GetFacturaFolder()
        {
            var folder = Path.Combine(
                _env.ContentRootPath,
                "Storage",
                "Facturas"
            );

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }
        public string BuildFacturaUrl(string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            var baseUrl = $"{request?.Scheme}://{request?.Host}";

            return $"{baseUrl}/api/factura/pdf/{fileName}";
        }
    }
}