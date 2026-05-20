using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.ViewModels;
using PrimePOS.WinUI.Views.Overlays;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services;



public class OverlayService
{
    private Grid? _container;
    private ContentControl? _content;

    private IOverlayViewModel? _currentVm;

    public bool IsOpen { get; private set; }

    public void Initialize(Grid container, ContentControl content)
    {
        _container = container;
        _content = content;
    }

    public async Task<bool> ShowAsync(
        UserControl overlay,
        IOverlayViewModel vm)
    {
        if (_container == null || _content == null)
            throw new InvalidOperationException("OverlayService no inicializado.");

        _currentVm = vm;

        _content.Content = overlay;
        _container.Visibility = Visibility.Visible;

        IsOpen = true;

        var result = await vm.WaitTask;

        CloseInternal();

        return result;
    }

    public void Close(bool result = false)
    {
        _currentVm?.Close(result);

        CloseInternal();
    }

    private void CloseInternal()
    {
        if (_container == null || _content == null)
            return;

        _content.Content = null;
        _container.Visibility = Visibility.Collapsed;

        IsOpen = false;

        _currentVm = null;
    }

    public async Task<bool> ConfirmAsync(string title, string message)
    {
        var vm = new DialogViewModel(title, message);

        var overlay = new DialogOverlay(vm);

        return await ShowAsync(overlay, vm);

    }

}
