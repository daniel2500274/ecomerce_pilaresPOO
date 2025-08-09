namespace EG2_pilares_POO.Interfaces
{ 
    /// Interfaz para métodos de pago 
    public interface IPago
    {
        bool ProcesarPago(decimal monto);
        string ObtenerTipoPago();
    }
}