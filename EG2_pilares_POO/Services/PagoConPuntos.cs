using System;
using EG2_pilares_POO.Interfaces;
using EG2_pilares_POO.Models;

namespace EG2_pilares_POO.Services
{ 
    /// Implementación de pago con puntos
    public class PagoConPuntos : IPago
    {
        private Cliente cliente;
        private decimal tasaConversion;  

        public PagoConPuntos(Cliente cliente, decimal tasaConversion = 10) // 10 puntos = GTQ 1.00
        {
            this.cliente = cliente;
            this.tasaConversion = tasaConversion;
        }

        public bool ProcesarPago(decimal monto)
        {
            Console.WriteLine("Procesando pago con puntos...");
            
            decimal puntosNecesarios = monto * tasaConversion;
            
            if (cliente.SaldoPuntos >= puntosNecesarios)
            {
                cliente.DescontarPuntos(puntosNecesarios);
                Console.WriteLine(string.Format("Pago exitoso por GTQ {0:F2} usando {1} puntos. Puntos restantes: {2}", 
                    monto, puntosNecesarios, cliente.SaldoPuntos));
                return true;
            }
            else
            {
                Console.WriteLine(string.Format("Puntos insuficientes. Necesarios: {0}, Disponibles: {1}", 
                    puntosNecesarios, cliente.SaldoPuntos));
                return false;
            }
        }

        public string ObtenerTipoPago()
        {
            return "Puntos de Recompensa";
        }
    }
}