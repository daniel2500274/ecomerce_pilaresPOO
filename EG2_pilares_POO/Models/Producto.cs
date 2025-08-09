using System;

namespace EG2_pilares_POO.Models
{ 
    /// Clase base abstracta para productos 
    public abstract class Producto
    {
        private string nombre;
        private decimal precio;
        private string categoria;
        private decimal descuento;

        public Producto(string nombre, decimal precio, string categoria, decimal descuento = 0)
        {
            this.nombre = nombre;
            this.precio = precio;
            this.categoria = categoria;
            this.descuento = descuento;
        }

        public string Nombre 
        { 
            get { return nombre; } 
            protected set { nombre = value; }
        }

        public decimal Precio 
        { 
            get { return precio; } 
            protected set { precio = value; }
        }

        public string Categoria 
        { 
            get { return categoria; } 
            protected set { categoria = value; }
        }

        public decimal Descuento 
        { 
            get { return descuento; } 
            set { descuento = value; }
        }

        public virtual decimal CalcularPrecioFinal()
        {
            return precio * (1 - descuento / 100);
        }

        public abstract decimal CalcularCostoEnvio();

        public override string ToString()
        {
            return string.Format("{0} - GTQ {1:F2} (Categoría: {2})", nombre, CalcularPrecioFinal(), categoria);
        }
    }
}