using PrimePOS.WinUI.Models;
using System.Collections.Generic;

namespace PrimePOS.WinUI.Helpers;

public static class GlyphCatalog
{
    public static List<GlyphItem> Todos =>
    [
        // TECNOLOGIA
        new("Telefonos", "\uE8EA"),
        new("Laptops", "\uE7F8"),
        new("PC", "\uE977"),
        new("Tablets", "\uE70A"),
        new("TV", "\uE7F4"),
        new("Cargadores", "\uECF0"),

        // COMIDA Y BEBIDAS
        new("Bebida", ""),
        new("Comida", ""),
        new("Cafe", ""),

        // NEGOCIO
        new("Caja", ""),
        new("Dinero", ""),
        new("Inventario", ""),
        new("Carrito", ""),
        new("Reporte", ""),

        // PERSONAS
        new("Cliente", ""),
        new("Usuario", ""),

        // OTROS
        new("Farmacia", ""),
        new("Hogar", ""),
        new("Ropa", ""),
        new("Zapatos", ""),
        new("Regalo", ""),
        new("Musica", ""),
        new("Juego", "")
    ];
}