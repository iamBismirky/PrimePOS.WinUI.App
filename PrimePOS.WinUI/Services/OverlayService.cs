using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace PrimePOS.WinUI.Services;



public class OverlayService
{
    private Grid? _container;
    private ContentControl? _content;

    public bool IsOpen { get; private set; }

    public void Initialize(Grid container, ContentControl content)
    {
        _container = container;
        _content = content;
    }

    public void Show(UserControl overlay)
    {
        if (_container == null || _content == null)
            throw new InvalidOperationException("OverlayService no inicializado.");

        _content.Content = overlay;
        _container.Visibility = Visibility.Visible;

        IsOpen = true;
    }

    public void Close()
    {
        if (_container == null || _content == null)
            return;

        _content.Content = null;
        _container.Visibility = Visibility.Collapsed;

        IsOpen = false;
    }
}
