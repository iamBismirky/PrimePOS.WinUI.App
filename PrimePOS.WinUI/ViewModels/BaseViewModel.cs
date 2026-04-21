using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI;

public class BasePage<TViewModel> : Page
    where TViewModel : class
{
    public TViewModel ViewModel { get; }

    public BasePage()
    {
        ViewModel = App.Services.GetRequiredService<TViewModel>();
        DataContext = ViewModel;
    }
}