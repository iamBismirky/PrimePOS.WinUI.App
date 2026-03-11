using System;
using System.Collections.Generic;
using System.Text;

namespace PrimePOS.WinUI.ViewModel
{
    using PrimePOS.BLL.DTOs.Producto;
    using PrimePOS.ENTITIES.Models;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class VentasViewModel
    {
        public ObservableCollection<CarritoItemVm> Carrito { get; set; } = new();

        public decimal Subtotal => Carrito.Sum(x => x.Total);

        public decimal Impuesto => Subtotal * 0.18m;

        public decimal Total => Subtotal + Impuesto;

        public void AgregarProducto(CarritoItemVm producto)
        {
            var existente = Carrito
                .FirstOrDefault(p => p.ProductoId == producto.ProductoId);

            if (existente != null)
            {
                existente.Cantidad++;
            }
            else
            {
                Carrito.Add(new CarritoItemVm
                {
                    ProductoId = producto.ProductoId,
                    Codigo = producto.Codigo,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1
                });
            }
        }

        public void QuitarProducto(CarritoItemVm item)
        {
            Carrito.Remove(item);
        }

        public void LimpiarCarrito()
        {
            Carrito.Clear();
        }

        
    }
}
