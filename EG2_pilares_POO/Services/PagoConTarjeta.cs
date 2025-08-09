using System;
using EG2_pilares_POO.Interfaces;

namespace EG2_pilares_POO.Services
{ 
    /// Implementación de pago con tarjeta de crédito 
    public class PagoConTarjeta : IPago
    {
        private string numeroTarjeta;
        private string nombreTitular;
        private decimal fondosDisponibles;

        public PagoConTarjeta(string numeroTarjeta, string nombreTitular, decimal fondosDisponibles)
        {
            this.numeroTarjeta = numeroTarjeta;
            this.nombreTitular = nombreTitular;
            this.fondosDisponibles = fondosDisponibles;
        }

        public bool ProcesarPago(decimal monto)
        {
            Console.WriteLine("Procesando pago con tarjeta...");
            
            if (fondosDisponibles >= monto)
            {
                fondosDisponibles -= monto;
                Console.WriteLine(string.Format("Pago exitoso por GTQ {0:F2}. Fondos restantes: GTQ {1:F2}", monto, fondosDisponibles));
                return true;
            }
            else
            {
                Console.WriteLine("Fondos insuficientes en la tarjeta");
                return false;
            }
        }

        public string ObtenerTipoPago()
        {
            return "Tarjeta de Crédito";
        }
    }
}