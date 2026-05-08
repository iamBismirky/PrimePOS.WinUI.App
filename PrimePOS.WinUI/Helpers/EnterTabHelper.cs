using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace PrimePOS.WinUI.Helpers;

public static class EnterAsTab
{
    public static void Attach(UIElement root)
    {
        root.AddHandler(
            UIElement.KeyDownEvent,
            new KeyEventHandler((s, e) =>
            {
                HandleKeyDown(root, e);
            }),
            true);
    }

    private static void HandleKeyDown(
        UIElement root,
        KeyRoutedEventArgs e)
    {
        if (e.Key != VirtualKey.Enter)
            return;

        var focused = FocusManager.GetFocusedElement();

        if (focused == null)
            return;

        // Ignorar AutoSuggestBox
        if (focused is AutoSuggestBox)
            return;

        // Ignorar multilinea
        if (focused is TextBox tb &&
            tb.AcceptsReturn)
            return;

        e.Handled = true;

        // BUTTON = CLICK
        if (focused is Button button)
        {
            if (button.Command?.CanExecute(null) == true)
            {
                button.Command.Execute(null);
            }

            return;
        }

        // ENTER = TAB
        var options = new FindNextElementOptions
        {
            SearchRoot = root
        };

        FocusManager.TryMoveFocus(
            FocusNavigationDirection.Next,
            options);
    }
}