using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.ViewModels;
using PrimePOS.WinUI.Views.Overlays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services;

public class OverlayService
{
    private Grid? _overlayHost;

    private readonly Stack<UserControl> _overlays = new();

    private readonly Dictionary<UserControl, IOverlayViewModel> _viewModels = new();

    public bool IsOpen { get; private set; }

    public void Initialize(Grid overlayHost)
    {
        _overlayHost = overlayHost;
    }

    public async Task<bool> ShowAsync(UserControl overlay, IOverlayViewModel vm)
    {
        if (_overlayHost == null)
            throw new InvalidOperationException("OverlayService no inicializado.");

        _viewModels[overlay] = vm;

        _overlayHost.Children.Add(overlay);

        _overlays.Push(overlay);

        // SOLO animar cuando es el primer overlay
        if (_overlays.Count == 1)
        {
            _overlayHost.Visibility = Visibility.Visible;

            FadeInHost();
        }

        IsOpen = true;

        var result = await vm.WaitTask;

        Close(overlay);

        return result;
    }

    public void Close(UserControl overlay)
    {
        if (_overlayHost == null)
            return;

        if (_viewModels.TryGetValue(overlay, out var vm))
        {
            vm.Close(false);

            _viewModels.Remove(overlay);
        }

        _overlayHost.Children.Remove(overlay);

        var temp = _overlays.ToList();

        temp.Remove(overlay);

        _overlays.Clear();

        foreach (var item in temp.Reverse<UserControl>())
            _overlays.Push(item);

        // SOLO ocultar cuando no quedan overlays
        if (_overlayHost.Children.Count == 0)
        {
            FadeOutHost();

            IsOpen = false;
        }
    }

    public void CloseTop()
    {
        if (_overlays.Count == 0)
            return;

        var overlay = _overlays.Pop();

        Close(overlay);
    }

    public async Task<bool> ConfirmAsync(string title, string message)
    {
        var vm = new DialogViewModel(title, message);

        var overlay = new DialogOverlay(vm);

        return await ShowAsync(overlay, vm);
    }

    public async Task<bool> ShowLoginAsync(LoginOverlayViewModel vm)
    {
        if (_overlayHost == null)
            throw new InvalidOperationException("Overlay no inicializado.");

        vm.ResetTask();

        var overlay = new LoginOverlay(vm);

        _viewModels[overlay] = vm;

        _overlayHost.Children.Add(overlay);

        _overlays.Push(overlay);

        // SOLO animar si es el primero
        if (_overlays.Count == 1 || _overlays.Count == 2)
        {
            _overlayHost.Visibility = Visibility.Visible;

            FadeInHost();
        }

        IsOpen = true;

        var result = await vm.WaitTask;

        Close(overlay);

        return result;
    }

    private void FadeInHost()
    {
        if (_overlayHost == null)
            return;

        _overlayHost.Opacity = 0;

        var animation = new DoubleAnimation
        {
            To = 1,
            Duration = TimeSpan.FromMilliseconds(150)
        };

        Storyboard.SetTarget(animation, _overlayHost);

        Storyboard.SetTargetProperty(animation, "Opacity");

        var storyboard = new Storyboard();

        storyboard.Children.Add(animation);

        storyboard.Begin();
    }

    private void FadeOutHost()
    {
        if (_overlayHost == null)
            return;

        var animation = new DoubleAnimation
        {
            To = 0,
            Duration = TimeSpan.FromMilliseconds(150)
        };

        Storyboard.SetTarget(animation, _overlayHost);

        Storyboard.SetTargetProperty(animation, "Opacity");

        var storyboard = new Storyboard();

        storyboard.Children.Add(animation);

        storyboard.Completed += (_, _) =>
        {
            if (_overlayHost != null)
            {
                _overlayHost.Visibility = Visibility.Collapsed;
            }
        };

        storyboard.Begin();
    }
}