using System;

namespace PrimePOS.WinUI.ViewModels
{
    public class FacturaViewModel
    {
        public string? UrlPdf { get; }

        public event Action? OnCerrar;

        public FacturaViewModel(string? urlPdf)
        {
            UrlPdf = urlPdf;
        }

        public void Cerrar()
        {
            OnCerrar?.Invoke();
        }
    }
}
