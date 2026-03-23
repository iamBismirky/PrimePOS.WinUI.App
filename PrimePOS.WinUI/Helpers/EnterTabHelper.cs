using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimePOS.WinUI.Helpers
{

    public static class EnterTabHelper
    {
        /// <summary>
        /// Activa Enter como Tab en toda la página.
        /// </summary>
        /// <param name="root">Elemento raíz de la página (this)</param>
        /// <param name="accionFinal">Acción a ejecutar en el último control</param>
        public static void Activar(UIElement root, Action? accionFinal = null)
        {
            root.KeyDown += (s, e) =>
            {
                if (e.Key != Windows.System.VirtualKey.Enter)
                    return;

                var controles = ObtenerControlesOrdenados(root);

                if (controles.Count == 0)
                    return;

                var focused = FocusManager.GetFocusedElement() as Control;
                if (focused == null)
                    return;

                int index = controles.IndexOf(focused);
                if (index == -1)
                    return;

                // Siguiente control
                if (index < controles.Count - 1)
                {
                    controles[index + 1].Focus(FocusState.Keyboard);
                }
                else
                {
                    // Último → ejecutar acción final
                    accionFinal?.Invoke();
                }

                e.Handled = true;
            };
        }

        // Obtiene todos los controles con TabIndex ordenados
        private static List<Control> ObtenerControlesOrdenados(DependencyObject parent)
        {
            var lista = new List<Control>();
            Recorrer(parent, lista);
            return lista
                .Where(c => c.IsTabStop)
                .OrderBy(c => c.TabIndex)
                .ToList();
        }

        // Recursivo para obtener todos los controles hijos
        private static void Recorrer(DependencyObject parent, List<Control> lista)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control control)
                {
                    lista.Add(control);
                }
                Recorrer(child, lista);
            }
        }
    }
}

