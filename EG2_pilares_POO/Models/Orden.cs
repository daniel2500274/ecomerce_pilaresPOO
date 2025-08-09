using System;
using System.Collections.Generic;
using EG2_pilares_POO.Enums;
using EG2_pilares_POO.Interfaces;

namespace EG2_pilares_POO.Models
{ 
    /// Orden de compra  
    public class Orden
    {
        private static int contadorOrdenes = 1;
        private int numeroOrden;
        private Cliente cliente;
        private List<Producto> productos;
        private decimal total;
        private DateTime fechaCreacion;
        private EstadoOrden estado;
        private IPago metodoPago;

        public Orden(Cliente cliente, Carrito carrito)
        {
            this.numeroOrden = contadorOrdenes++;
            this.cliente = cliente;
            this.productos = carrito.ObtenerProductos();
            this.total = carrito.CalcularTotal();
            this.fechaCreacion = DateTime.Now;
            this.estado = EstadoOrden.Pendiente;
        }

        public int NumeroOrden 
        { 
            get { return numeroOrden; } 
        }

        public EstadoOrden Estado 
        { 
            get { return estado; } 
            private set { estado = value; }
        }

        public void AsignarMetodoPago(IPago metodoPago)
        {
            this.metodoPago = metodoPago;
        }

        public void MostrarResumen()
        {
            Console.WriteLine("================================");
            Console.WriteLine("        RESUMEN DE ORDEN        ");
            Console.WriteLine("================================");
            Console.WriteLine(string.Format("Número de Orden: {0}", numeroOrden));
            Console.WriteLine(string.Format("Cliente: {0}", cliente.Nombre));
            Console.WriteLine(string.Format("Email: {0}", cliente.Email));
            Console.WriteLine(string.Format("Fecha: {0}", fechaCreacion.ToString("dd/MM/yyyy HH:mm")));
            Console.WriteLine(string.Format("Estado: {0}", estado));
            Console.WriteLine();

            Console.WriteLine("PRODUCTOS:");
            Console.WriteLine("----------");
            foreach (Producto producto in productos)
            {
                Console.WriteLine(string.Format("• {0} - GTQ {1:F2}", producto.Nombre, producto.CalcularPrecioFinal()));
            }

            Console.WriteLine();
            Console.WriteLine(string.Format("TOTAL: GTQ {0:F2}", total));
            
            if (metodoPago != null)
            {
                Console.WriteLine(string.Format("Método de Pago: {0}", metodoPago.ObtenerTipoPago()));
            }
            Console.WriteLine("================================");
        }

        public bool ConfirmarCompra()
        {
            if (estado != EstadoOrden.Pendiente)
            {
                Console.WriteLine("La orden ya fue procesada");
                return false;
            }

            if (metodoPago == null)
            {
                Console.WriteLine("Debe asignar un método de pago");
                return false;
            }

            if (metodoPago.ProcesarPago(total))
            {
                estado = EstadoOrden.Confirmada;
                Console.WriteLine(string.Format("¡Orden #{0} confirmada exitosamente!", numeroOrden));
                
                // Agregar puntos de recompensa al cliente (1% del total)
                cliente.AgregarPuntos(total * 0.01m * 10);   
                
                // Generar links de descarga para productos digitales
                foreach (Producto producto in productos)
                {
                    if (producto is ProductoDigital)
                    {
                        ProductoDigital productoDigital = (ProductoDigital)producto;
                        productoDigital.GenerarLinkDescarga();
                        Console.WriteLine(string.Format("Link de descarga generado para {0}: {1}", 
                            producto.Nombre, productoDigital.LinkDescarga));
                    }
                }
                
                return true;
            }
            else
            {
                Console.WriteLine("Error en el procesamiento del pago");
                return false;
            }
        }

        public void CancelarOrden()
        {
            if (estado == EstadoOrden.Pendiente || estado == EstadoOrden.Confirmada)
            {
                estado = EstadoOrden.Cancelada;
                Console.WriteLine(string.Format("Orden #{0} cancelada", numeroOrden));
            }
            else
            {
                Console.WriteLine("No se puede cancelar la orden en su estado actual");
            }
        }
    }
}