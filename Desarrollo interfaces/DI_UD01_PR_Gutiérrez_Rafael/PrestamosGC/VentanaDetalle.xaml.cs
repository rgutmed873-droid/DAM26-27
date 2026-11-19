using System.Windows;
using PrestamosGC.Datos;

namespace PrestamosGC;

// ETIQUETAS: [ENUNCIADO] = lo pide el PDF de la práctica pero no está en los apuntes
//            [EXTRA]     = lo he añadido yo por mi cuenta
//            Sin etiqueta = está en los apuntes

/// <summary>
/// VENTANA DE DETALLE de un préstamo. Se abre como ventana modal.
/// Si se registra la devolución, DialogResult vale true y la ventana principal recarga la lista.
/// </summary>
public partial class VentanaDetalle : Window
{
    // CONSTRUCTOR con parámetro: la ventana principal le pasa el préstamo que quiere ver
    public VentanaDetalle(Prestamo prestamo)
    {
        InitializeComponent();

        // DataContext = "el objeto de donde sacan los datos todos los {Binding ...} del XAML".
        // Se hereda: lo usan todos los controles de la ventana (apuntes 2.7).
        DataContext = prestamo;
    }

    /// <summary>Botón «Registrar devolución».</summary>
    private void Devolucion_Click(object sender, RoutedEventArgs e)
    {
        // DataContext es de tipo object, así que lo convertimos (cast) a Prestamo
        Prestamo prestamo = (Prestamo)DataContext;

        // Pedimos confirmación con un MessageBox de Sí/No (apuntes 2.5)
        MessageBoxResult respuesta = MessageBox.Show(
            this,
            "¿Registrar la devolución del portátil " + prestamo.Portatil + " de " + prestamo.NombreCompleto + "?",
            "Confirmar devolución",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        // Si no ha pulsado «Sí», no hacemos nada
        if (respuesta != MessageBoxResult.Yes) return;

        // RegistrarDevolucion puede lanzar una excepción, así que va dentro de un try/catch
        try
        {
            App.Prestamos.RegistrarDevolucion(prestamo.Id);
        }
        // [EXTRA] "when (...)" = este catch solo atrapa esos tres tipos de error:
        //   InvalidOperationException = el préstamo no existe o ya estaba devuelto
        //   IOException / UnauthorizedAccessException = problemas al guardar el archivo
        catch (Exception ex) when (ex is InvalidOperationException or IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, ex.Message, "No se pudo registrar la devolución",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Todo bien: cerramos devolviendo true (así la ventana principal sabe que debe recargar la lista)
        DialogResult = true;
    }
}
