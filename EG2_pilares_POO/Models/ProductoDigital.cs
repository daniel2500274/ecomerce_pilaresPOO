using System;

namespace EG2_pilares_POO.Models
{ 
    /// Producto digital sin costo de envío 
    public class ProductoDigital : Producto
    {
        private string linkDescarga;
        private DateTime fechaVencimientoLicencia;

        public ProductoDigital(string nombre, decimal precio, string categoria, DateTime fechaVencimiento, decimal descuento = 0) 
            : base(nombre, precio, categoria, descuento)
        {
            this.fechaVencimientoLicencia = fechaVencimiento;
        }

        public string LinkDescarga 
        { 
            get { return linkDescarga; } 
            set { linkDescarga = value; }
        }

        public DateTime FechaVencimientoLicencia 
        { 
            get { return fechaVencimientoLicencia; } 
            set { fechaVencimientoLicencia = value; }
        }

        public override decimal CalcularCostoEnvio()
        {
            return 0; // Los productos digitales no tienen costo de envío
        }

        public void GenerarLinkDescarga()
        {
            linkDescarga = string.Format("https://linkdescarga.ejemplo.com/{0}", Guid.NewGuid());
        }
    }
}