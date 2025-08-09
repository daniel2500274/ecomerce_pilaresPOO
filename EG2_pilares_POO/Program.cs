using System;
using EG2_pilares_POO.Models;
using EG2_pilares_POO.Services;
using EG2_pilares_POO.Enums;

namespace EG2_pilares_POO
{ 
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE E-COMMERCE ===");
            Console.WriteLine();

            // Crear cliente
            Cliente cliente = new Cliente("Juan Pérez", "juan@email.com", "Calle 123, Ciudad", 500);

            // Crear productos
            ProductoFisico laptop = new ProductoFisico("Laptop Gaming", 15000, "Electrónicos", 2.5m, 10);
            ProductoFisico mouse = new ProductoFisico("Mouse Inalámbrico", 800, "Electrónicos", 0.2m);
            ProductoDigital software = new ProductoDigital("Antivirus Premium", 1200, "Software", 
                DateTime.Now.AddYears(1), 15);

            // Crear carrito y agregar productos
            Carrito carrito = new Carrito();
            carrito.AgregarProducto(laptop);
            carrito.AgregarProducto(mouse);
            carrito.AgregarProducto(software);

            // Mostrar carrito
            carrito.ListarProductos();
            Console.WriteLine();
            Console.WriteLine(string.Format("Subtotal: GTQ {0:F2}", carrito.CalcularSubtotal()));
            Console.WriteLine(string.Format("Impuestos: GTQ {0:F2}", carrito.CalcularImpuestos()));
            Console.WriteLine(string.Format("Total: GTQ {0:F2}", carrito.CalcularTotal()));
            Console.WriteLine();

            // Crear orden
            Orden orden = new Orden(cliente, carrito);
            orden.MostrarResumen();

            // Demostrar diferentes métodos de pago
            Console.WriteLine("\n=== PROBANDO MÉTODOS DE PAGO ===");

            // 1. Pago con tarjeta (con fondos insuficientes)
            PagoConTarjeta pagoTarjeta = new PagoConTarjeta("1234-5678-9012-3456", "Juan Pérez", 10000);
            orden.AsignarMetodoPago(pagoTarjeta);
            Console.WriteLine("\n--- Intento 1: Pago con Tarjeta ---");
            orden.ConfirmarCompra(); 

            // 2. Pago con transferencia ( 
            PagoConTransferencia pagoTransferencia = new PagoConTransferencia("001-123456", "Banco Nacional", 20000);
            orden.AsignarMetodoPago(pagoTransferencia);
            Console.WriteLine("\n--- Intento 2: Pago con Transferencia ---");
            
            // Crear nueva orden porque la anterior falló
            Orden orden2 = new Orden(cliente, carrito);
            orden2.AsignarMetodoPago(pagoTransferencia);
            orden2.ConfirmarCompra();  

            Console.WriteLine(string.Format("\nPuntos del cliente después de la compra: {0}", cliente.SaldoPuntos));

            // 3. Pago con puntos
            Console.WriteLine("\n--- Demostración: Pago con Puntos ---");
            cliente.AgregarPuntos(200000); 
            
            Carrito carritoChico = new Carrito();
            carritoChico.AgregarProducto(mouse);
            
            Orden ordenPuntos = new Orden(cliente, carritoChico);
            PagoConPuntos pagoPuntos = new PagoConPuntos(cliente);
            ordenPuntos.AsignarMetodoPago(pagoPuntos);
            ordenPuntos.ConfirmarCompra();
        }
    }
}