using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace PrimePOS.WinUI.Services;

public static class KeyboardService
{
    public static void Attach(UIElement element)
    {
        element.AddHandler(
            UIElement.KeyDownEvent,
            new KeyEventHandler(OnKeyDown),
            true);
    }

    private static void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key != VirtualKey.Enter)
            return;

        var focused = FocusManager.GetFocusedElement() as FrameworkElement;

        if (focused == null)
            return;

        switch (focused)
        {
            case AutoSuggestBox:
                return;

            case TextBox tb when tb.AcceptsReturn:
                return;

            case TextBox:
            case NumberBox:

                e.Handled = true;

                FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                break;

            case Button button:

                e.Handled = true;

                if (button.Command?.CanExecute(null) == true)
                {
                    button.Command.Execute(null);
                }

                break;
        }
    }
}