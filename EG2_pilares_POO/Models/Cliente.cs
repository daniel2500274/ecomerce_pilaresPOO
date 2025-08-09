namespace EG2_pilares_POO.Models
{ 
    /// Cliente del e-commerce, clase modelo
    public class Cliente
    {
        private string nombre;
        private string email;
        private string direccion;
        private decimal saldoPuntos;

        public Cliente(string nombre, string email, string direccion, decimal saldoPuntos = 0)
        {
            this.nombre = nombre;
            this.email = email;
            this.direccion = direccion;
            this.saldoPuntos = saldoPuntos;
        }

        public string Nombre 
        { 
            get { return nombre; } 
            set { nombre = value; }
        }

        public string Email 
        { 
            get { return email; } 
            set { email = value; }
        }

        public string Direccion 
        { 
            get { return direccion; } 
            set { direccion = value; }
        }

        public decimal SaldoPuntos 
        { 
            get { return saldoPuntos; } 
            set { saldoPuntos = value; }
        }

        public void AgregarPuntos(decimal puntos)
        {
            saldoPuntos += puntos;
        }

        public bool DescontarPuntos(decimal puntos)
        {
            if (saldoPuntos >= puntos)
            {
                saldoPuntos -= puntos;
                return true;
            }
            return false;
        }
    }
}