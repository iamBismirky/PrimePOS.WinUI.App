using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PrimePOS.WinUI.Views.Overlays
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginOverlay : UserControl
    {
        public LoginOverlay(LoginOverlayViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
        private void pwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginOverlayViewModel vm)
            {
                vm.Password = pwdPassword.Password;
            }

        }
        private void Control_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;

            switch (sender)
            {
                case TextBox tb when tb == txtUsername:

                    pwdPassword.Focus(
                        FocusState.Programmatic);

                    break;

                case PasswordBox pb when pb == pwdPassword:

                    btnLogin.Focus(
                        FocusState.Programmatic);

                    break;
            }

        }
    }
}
