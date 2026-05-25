using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.WinUI.Contracts;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class DialogOverlayViewModel : ObservableObject, IOverlayViewModel
{
    private readonly TaskCompletionSource<bool> _tcs = new();

    public string Title { get; }
    public string Message { get; }

    public Task<bool> WaitTask => _tcs.Task;

    public DialogOverlayViewModel(string title, string message)
    {
        Title = title;
        Message = message;
    }

    [RelayCommand]
    private void Confirmar()
    {
        Close(true);
    }

    [RelayCommand]
    private void Cancelar()
    {
        Close(false);
    }

    public void Close(bool result)
    {
        _tcs.TrySetResult(result);
    }
}