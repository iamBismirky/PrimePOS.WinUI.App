using System;

namespace PrimePOS.WinUI.Models
{
    public class AppNotification
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public AppNotificationType Type { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public bool Leida { get; set; }

        public string? Modulo { get; set; }
    }

}
