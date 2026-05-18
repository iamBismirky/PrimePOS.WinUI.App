using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.ViewModels;
using PrimePOS.WinUI.ViewModels.Overlays;
using PrimePOS.WinUI.Views.Overlays;
using System;
using System.Threading.Tasks;

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
    public async Task<bool> ConfirmAsync(string title, string message)
    {
        var vm = new DialogViewModel(title, message);

        var overlay = new DialogOverlay(vm);

        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }
    public async Task<bool> ShowClienteAsync(ClienteOverlay overlay, ClienteOverlayViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    public async Task<bool> ShowCajaAsync(CajaOverlay overlay, CajaOverlayViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    internal async Task<bool> ShowCategoriaAsync(CategoriaOverlay overlay, CategoriaOverlayViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    public async Task<bool> ShowRolAsync(RolOverlay overlay, RolOverlayViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    public async Task<bool> ShowUsuarioAsync(UsuarioOverlay overlay, UsuarioOverlayViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }
    public async Task<bool> ShowProductoAsync(ProductoOverlay overlay, ProductoOverlayViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    public async Task<bool> ShowTurnoAsync(AbrirTurnoOverlay overlay, AbrirTurnoViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    public async Task<bool> ShowCerrarTurnoAsync(CerrarTurnoOverlay overlay, CerrarTurnoViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }

    public async Task<bool> ShowCobrarAsync(CobrarOverlay overlay, CobrarViewModel vm)
    {
        Show(overlay);

        var result = await vm.WaitTask;

        Close();

        return result;
    }
}
