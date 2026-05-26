using PrimePOS.WinUI.Models;
using System;
using System.Collections.ObjectModel;

namespace PrimePOS.WinUI.Services;

public class NotificationService
{
    public ObservableCollection<AppNotification>
        Notifications
    { get; } = new();

    public event Action<AppNotification>?
        OnNotification;

    public void Success(string message)
    {
        Show(
            "Éxito",
            message,
            AppNotificationType.Success);
    }

    public void Error(string message)
    {
        Show(
            "Error",
            message,
            AppNotificationType.Error);
    }

    public void Warning(string message)
    {
        Show(
            "Advertencia",
            message,
            AppNotificationType.Warning);
    }

    public void Info(string message)
    {
        Show(
            "Información",
            message,
            AppNotificationType.Info);
    }

    private void Show(
        string title,
        string message,
        AppNotificationType type)
    {
        var notification = new AppNotification
        {
            Title = title,
            Message = message,
            Type = type
        };

        // Última notificación arriba
        Notifications.Insert(0, notification);

        // Evento global
        OnNotification?.Invoke(notification);
    }
}