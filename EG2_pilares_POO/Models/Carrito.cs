using System;
using System.Collections.Generic;
using System.Linq;

namespace EG2_pilares_POO.Models
{ 
    /// Carrito de compras 
    public class Carrito
    {
        private List<Producto> productos;
        private decimal tasaImpuesto;

        public Carrito(decimal tasaImpuesto = 0.12m) //IVA 
        {
            productos = new List<Producto>();
            this.tasaImpuesto = tasaImpuesto;
        }

        public void AgregarProducto(Producto producto)
        {
            productos.Add(producto);
            Console.WriteLine("Producto agregado: " + producto.Nombre);
        }

        public bool EliminarProducto(string nombreProducto)
        {
            Producto producto = productos.FirstOrDefault(p => p.Nombre == nombreProducto);
            if (producto != null)
            {
                productos.Remove(producto);
                Console.WriteLine("Producto eliminado: " + nombreProducto);
                return true;
            }
            Console.WriteLine("Producto no encontrado: " + nombreProducto);
            return false;
        }

        public void ListarProductos()
        {
            Console.WriteLine("=== PRODUCTOS EN CARRITO ===");
            if (productos.Count == 0)
            {
                Console.WriteLine("El carrito está vacío");
                return;
            }

            for (int i = 0; i < productos.Count; i++)
            {
                Console.WriteLine(string.Format("{0}. {1}", i + 1, productos[i]));
            }
        }

        public decimal CalcularSubtotal()
        {
            decimal subtotal = 0;
            foreach (Producto producto in productos)
            {
                subtotal += producto.CalcularPrecioFinal();
            }
            return subtotal;
        }

        public decimal CalcularImpuestos()
        {
            return CalcularSubtotal() * tasaImpuesto;
        }

        public decimal CalcularTotal()
        {
            return CalcularSubtotal() + CalcularImpuestos();
        }

        public List<Producto> ObtenerProductos()
        {
            return new List<Producto>(productos); 
        }

        public void VaciarCarrito()
        {
            productos.Clear();
        }
    }
}