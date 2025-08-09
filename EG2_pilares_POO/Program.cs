using System;
using System.Collections.Generic;
using System.Linq;

// Interfaz para métodos de pago
public interface IPago
{
    bool ProcesarPago(decimal monto);
    string ObtenerTipoPago();
}

// Clase base abstracta para productos
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

// Producto con envio
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

// Producto digital sin costo de envío
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

// Cliente del e-commerce
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

// Carrito de compras
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

// Implementaciones de métodos de pago
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


public enum EstadoOrden
{
    Pendiente,
    Confirmada,
    Cancelada,
    Enviada,
    Entregada
}


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

// Programa principal para demostrar el sistema
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== SISTEMA DE E-COMMERCE ===");
        Console.WriteLine();

        // Crear cliente
        Cliente cliente = new Cliente("Juan Pérez", "juan@email.com", "Calle 123, Ciudad", 500);

        // Crear productos
        ProductoFisico laptop = new ProductoFisico("Laptop Gaming", 15000, "Electrónicos", 2.5m, 10);
        ProductoFisico mouse = new ProductoFisico("Mouse Inalámbrico", 800, "Electrónicos", 0.2m);
        ProductoDigital software = new ProductoDigital("Antivirus Premium", 1200, "Software", 
            DateTime.Now.AddYears(1), 15);

        // Crear carrito y agregar productos
        Carrito carrito = new Carrito();
        carrito.AgregarProducto(laptop);
        carrito.AgregarProducto(mouse);
        carrito.AgregarProducto(software);

        // Mostrar carrito
        carrito.ListarProductos();
        Console.WriteLine();
        Console.WriteLine(string.Format("Subtotal: GTQ {0:F2}", carrito.CalcularSubtotal()));
        Console.WriteLine(string.Format("Impuestos: GTQ {0:F2}", carrito.CalcularImpuestos()));
        Console.WriteLine(string.Format("Total: GTQ {0:F2}", carrito.CalcularTotal()));
        Console.WriteLine();

        // Crear orden
        Orden orden = new Orden(cliente, carrito);
        orden.MostrarResumen();

        // Demostrar diferentes métodos de pago
        Console.WriteLine("\n=== PROBANDO MÉTODOS DE PAGO ===");

        // 1. Pago con tarjeta (con fondos insuficientes)
        PagoConTarjeta pagoTarjeta = new PagoConTarjeta("1234-5678-9012-3456", "Juan Pérez", 10000);
        orden.AsignarMetodoPago(pagoTarjeta);
        Console.WriteLine("\n--- Intento 1: Pago con Tarjeta ---");
        orden.ConfirmarCompra(); // Fallará por fondos insuficientes

        // 2. Pago con transferencia (exitoso)
        PagoConTransferencia pagoTransferencia = new PagoConTransferencia("001-123456", "Banco Nacional", 20000);
        orden.AsignarMetodoPago(pagoTransferencia);
        Console.WriteLine("\n--- Intento 2: Pago con Transferencia ---");
        
        // Crear nueva orden porque la anterior falló
        Orden orden2 = new Orden(cliente, carrito);
        orden2.AsignarMetodoPago(pagoTransferencia);
        orden2.ConfirmarCompra(); // Será exitoso

        Console.WriteLine(string.Format("\nPuntos del cliente después de la compra: {0}", cliente.SaldoPuntos));

        // 3. Demostrar pago con puntos
        Console.WriteLine("\n--- Demostración: Pago con Puntos ---");
        cliente.AgregarPuntos(200000); // Agregar más puntos para demostrar
        
        Carrito carritoChico = new Carrito();
        carritoChico.AgregarProducto(mouse);
        
        Orden ordenPuntos = new Orden(cliente, carritoChico);
        PagoConPuntos pagoPuntos = new PagoConPuntos(cliente);
        ordenPuntos.AsignarMetodoPago(pagoPuntos);
        ordenPuntos.ConfirmarCompra();
 
    }
}