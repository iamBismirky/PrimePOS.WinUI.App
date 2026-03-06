using Microsoft.EntityFrameworkCore;
using PrimePOS.DAL.Context;
using PrimePOS.DAL.Repositories;
using PrimePOS.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePOS.BLL.Services;

public class VentaService
{
    private readonly VentaRepository _ventaRepository;
    private readonly ProductoRepository _productoRepository;
    public VentaService(VentaRepository ventaRepository, ProductoRepository productoRepository)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
    }
    private const decimal ITBIS = 0.18m;
    public void AgregarProducto(Venta venta, string buscarProducto)
    {
        if (venta == null) throw new ArgumentNullException(nameof(venta));
        if (string.IsNullOrWhiteSpace(buscarProducto)) throw new ArgumentNullException(nameof(buscarProducto));

        // Asegurar que la colección no sea nula antes de usarla
        venta.Detalles ??= new List<DetalleVenta>();

        // 1️⃣ Obtener producto desde repository
        var producto = _productoRepository.BuscarPorCodigoONombre(buscarProducto);

        if (producto == null)
            throw new Exception("Producto no existe");

        var detalle = venta.Detalles
            .FirstOrDefault(d => d.ProductoId == producto.ProductoId);

        if (detalle != null)
        {
            detalle.Cantidad++;
        }
        else
        {
            venta.Detalles.Add(new DetalleVenta
            {
                ProductoId = producto.ProductoId,
                NombreProducto = producto.Nombre,
                PrecioUnitario = producto.PrecioVenta,
                Cantidad = 1
            });
        }

        CalcularTotales(venta);
    }
    public void AgregarVenta(Venta venta, List<DetalleVenta> detalles)
    {
        if (venta == null) throw new ArgumentNullException(nameof(venta));
        if (detalles == null) throw new ArgumentNullException(nameof(detalles));

        decimal subtotalGeneral = 0;
        const decimal impuestoGeneral = 0.18m;
        decimal totalGeneral = 0;

        foreach (var detalle in detalles)
        {
            var producto = _productoRepository.ObtenerPorId(detalle.ProductoId);
            if (producto == null)
                throw new Exception("Producto  no encontrado.");

            if (producto.Existencia < detalle.Cantidad)
                throw new Exception("Stock insuficiente");

            detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
            detalle.Impuesto = detalle.Subtotal * impuestoGeneral;

            subtotalGeneral += detalle.Subtotal;
            totalGeneral += detalle.Subtotal + detalle.Impuesto;

            producto.Existencia -= detalle.Cantidad;
            _productoRepository.Actualizar(producto);
        }

        venta.Total = totalGeneral;
        venta.Detalles = detalles;

        _ventaRepository.Agregar(venta);

       
        _ventaRepository.GuardarCambios();
    }
    public void CalcularTotales(Venta venta)
    {
        if (venta == null) throw new ArgumentNullException(nameof(venta));

        // Si no hay detalles, asegurar valores a 0 y evitar enumeración sobre null
        if (venta.Detalles == null || !venta.Detalles.Any())
        {
            venta.Subtotal = 0;
            venta.TotalImpuesto = 0;
            venta.Total = 0;
            return;
        }

        decimal subtotal = 0;
        decimal tasaImpuesto = 0.18m;
        decimal totalImpuesto = 0;

        foreach (var d in venta.Detalles)
        {
            d.Subtotal = d.Cantidad * d.PrecioUnitario;
            d.Impuesto = d.Subtotal * tasaImpuesto;
            d.Total = d.Subtotal + d.Impuesto;

            subtotal += d.Subtotal;
            totalImpuesto += d.Impuesto;
        }

        venta.Subtotal = subtotal;
        venta.TotalImpuesto = totalImpuesto;
        venta.Total = subtotal + totalImpuesto;
    }
}

