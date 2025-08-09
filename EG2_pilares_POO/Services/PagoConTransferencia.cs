using System;
using EG2_pilares_POO.Interfaces;

namespace EG2_pilares_POO.Services
{ 
    /// Implementación de pago con transferencia bancaria 
    public class PagoConTransferencia : IPago
    {
        private string numeroCuenta;
        private string banco;
        private decimal saldoCuenta;

        public PagoConTransferencia(string numeroCuenta, string banco, decimal saldoCuenta)
        {
            this.numeroCuenta = numeroCuenta;
            this.banco = banco;
            this.saldoCuenta = saldoCuenta;
        }

        public bool ProcesarPago(decimal monto)
        {
            Console.WriteLine("Procesando transferencia bancaria...");
            
            // Validación adicional para transferencias (monto mínimo)
            if (monto < 100)
            {
                Console.WriteLine("El monto mínimo para transferencia es GTG100");
                return false;
            }

            if (saldoCuenta >= monto)
            {
                saldoCuenta -= monto;
                Console.WriteLine(string.Format("Transferencia exitosa por GTQ {0:F2}. Saldo restante: GTQ {1:F2}", monto, saldoCuenta));
                return true;
            }
            else
            {
                Console.WriteLine("Saldo insuficiente en la cuenta");
                return false;
            }
        }

        public string ObtenerTipoPago()
        {
            return "Transferencia Bancaria";
        }
    }
}