namespace EG2_pilares_POO.Models
{ 
    /// Producto físico con costo de envío 
    public class ProductoFisico : Producto
    {
        private decimal peso;
        private string direccionEnvio;

        public ProductoFisico(string nombre, decimal precio, string categoria, decimal peso, decimal descuento = 0) 
            : base(nombre, precio, categoria, descuento)
        {
            this.peso = peso;
        }

        public decimal Peso 
        { 
            get { return peso; } 
            set { peso = value; }
        }

        public string DireccionEnvio 
        { 
            get { return direccionEnvio; } 
            set { direccionEnvio = value; }
        }

        public override decimal CalcularCostoEnvio()
        {
            // Costo base + peso * tarifa por kg
            return 10 + (peso * 2);
        }

        public override decimal CalcularPrecioFinal()
        {
            return base.CalcularPrecioFinal() + CalcularCostoEnvio();
        }
    }
}