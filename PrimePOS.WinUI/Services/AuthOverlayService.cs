using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PrimePOS.WinUI.Views.Overlays;
using System;
using System.Threading.Tasks;

namespace PrimePOS.WinUI.Services
{
    public class AuthOverlayService
    {
        private Grid? _container;
        private ContentControl? _content;

        public void Initialize(Grid container, ContentControl content)
        {
            _container = container;
            _content = content;
        }

        public async Task<bool> ShowLoginAsync(LoginOverlayViewModel vm)
        {
            if (_container == null || _content == null)
                throw new InvalidOperationException("Overlay no inicializado.");

            vm.ResetTask();

            var view = new LoginOverlay(vm);

            _content.Content = view;
            _container.Visibility = Visibility.Visible;

            var result = await vm.WaitTask;

            Close();

            return result;
        }

        public void Close()
        {
            if (_content != null)
                _content.Content = null;

            if (_container != null)
                _container.Visibility = Visibility.Collapsed;
        }
    }
}