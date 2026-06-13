using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace PrimePOS.WinUI.ViewModels.Pages
{
    public partial class EmpresaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? nombr;
        [ObservableProperty]
        private string? rnc;
        [ObservableProperty]
        private string? telefono;
        [ObservableProperty]
        private string? email;
        [ObservableProperty]
        private string? direccion;
        [ObservableProperty]
        private Image? logo;
        [ObservableProperty]
        private bool estado;
    }
}
