using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.ViewModels;

public partial class DialogViewModel : ObservableObject
{
    private readonly TaskCompletionSource<bool> _tcs = new();

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string message = string.Empty;

    public DialogViewModel(
        string title,
        string message)
    {
        Title = title;
        Message = message;
    }

    [RelayCommand]
    private void Confirm()
    {
        _tcs.TrySetResult(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        _tcs.TrySetResult(false);
    }

    public Task<bool> WaitTask => _tcs.Task;
}