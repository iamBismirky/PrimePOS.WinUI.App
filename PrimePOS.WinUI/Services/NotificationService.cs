using Microsoft.UI.Xaml.Controls;
using System;

namespace PrimePOS.WinUI.Services
{
    public class NotificationService
    {
        public event Action<string, InfoBarSeverity>? OnNotify;

        public void Success(string msg, int seconds = 3)
            => Raise(msg, InfoBarSeverity.Success, seconds);

        public void Error(string msg, int seconds = 3)
            => Raise(msg, InfoBarSeverity.Error, seconds);

        public void Warning(string msg, int seconds = 3)
            => Raise(msg, InfoBarSeverity.Warning, seconds);

        private void Raise(string msg, InfoBarSeverity severity, int seconds)
        {
            OnNotify?.Invoke(msg, severity);
        }
    }
}