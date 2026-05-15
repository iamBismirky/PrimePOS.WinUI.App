using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimePOS.WinUI.ViewModels;

public partial class DialogViewModel : ObservableObject
{
    private Func<Task> _onConfirm;
    private Action _onCancel;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string message = string.Empty;

    public void Initialize(
        string title,
        string message,
        Func<Task> onConfirm,
        Action onCancel)
    {
        Title = title;
        Message = message;

        _onConfirm = onConfirm;
        _onCancel = onCancel;
    }

    [RelayCommand]
    public async Task ConfirmAsync()
    {
        if (_onConfirm != null)
            await _onConfirm();
    }

    [RelayCommand]
    public void Cancel()
    {
        _onCancel?.Invoke();
    }
}