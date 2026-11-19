// Los "using" dicen qué carpetas de clases vamos a utilizar
using System.ComponentModel;   // CancelEventArgs (para cancelar el cierre)
using System.Windows;          // Window, MessageBox, RoutedEventArgs...
using System.Windows.Controls; // ListBox, SelectionChangedEventArgs...
using System.Windows.Input;    // MouseButtonEventArgs (doble clic)
using PrestamosGC.Datos;       // nuestra clase Prestamo

namespace PrestamosGC;

// ETIQUETAS: [ENUNCIADO] = lo pide el PDF de la práctica pero no está en los apuntes
//            [EXTRA]     = lo he añadido yo por mi cuenta
//            Sin etiqueta = está en los apuntes

/// <summary>
/// VENTANA PRINCIPAL. Aquí está el código (C#) de la ventana. El diseño está en VentanaPrincipal.xaml.
/// "partial" = la clase está repartida entre el .xaml y este archivo (apuntes 2.2).
/// </summary>
public partial class VentanaPrincipal : Window
{
    // CONSTRUCTOR: se ejecuta al crear la ventana
    public VentanaPrincipal()
    {
        InitializeComponent();   // carga el XAML y crea todos los controles (siempre va primero)
        CargarLista();           // rellena la lista y la barra de estado al abrir
    }

    // ------------------------------------------------------------------
    // LISTA Y FILTROS
    // ------------------------------------------------------------------

    /// <summary>
    /// Rellena la lista con los préstamos del filtro marcado y actualiza la barra de estado.
    /// Se llama al abrir la ventana y cada vez que algo cambia.
    /// </summary>
    private void CargarLista()
    {
        // 1) Pedimos todos los préstamos al almacén (clase GestorPrestamos)
        List<Prestamo> todos = App.Prestamos.ObtenerPrestamos();

        // 2) Nos quedamos solo con los del filtro marcado.
        //    [EXTRA] Where(...) es LINQ: "quédate con los que cumplan la condición".
        //    (LINQ no sale en los apuntes de esta unidad.)
        IEnumerable<Prestamo> visibles = todos;
        if (rbActivos.IsChecked == true)
            visibles = todos.Where(p => p.EstaActivo && !p.EstaVencido);   // en préstamo y dentro de plazo
        else if (rbVencidos.IsChecked == true)
            visibles = todos.Where(p => p.EstaVencido);                    // fuera de plazo
        else if (rbDevueltos.IsChecked == true)
            visibles = todos.Where(p => p.EstaDevuelto);                   // ya devueltos
        // (si está marcado "Todos" no se filtra nada)

        // 3) ItemsSource = la colección que muestra la lista (apuntes 2.5)
        lstPrestamos.ItemsSource = visibles.ToList();

        // 4) Barra de estado. Se cuenta sobre TODOS los préstamos, no sobre el filtro.
        int enPrestamo = todos.Count(p => p.EstaActivo && !p.EstaVencido);
        int vencidos = todos.Count(p => p.EstaVencido);
        int disponibles = App.Prestamos.PortatilesDisponibles().Count;

        // [EXTRA] Ajustamos singular/plural ("1 vencido" / "2 vencidos")
        string textoVencidos = vencidos == 1 ? "vencido" : "vencidos";
        string textoDisponibles = disponibles == 1 ? "portátil disponible" : "portátiles disponibles";

        txtEstado.Text = enPrestamo + " en préstamo · " +
                         vencidos + " " + textoVencidos + " · " +
                         disponibles + " " + textoDisponibles +
                         " de " + App.Prestamos.TotalPortatiles;
    }

    /// <summary>
    /// UN SOLO manejador para los 4 RadioButton (lo pide el ejercicio 6a).
    /// Se ejecuta cada vez que se marca uno de ellos.
    /// </summary>
    private void Filtro_Checked(object sender, RoutedEventArgs e)
    {
        // [EXTRA] Truco: al construir la ventana, "Todos" ya viene marcado en el XAML y este
        // método se dispara antes de que exista la lista -> daría error.
        // IsInitialized es false mientras se está construyendo, así que en ese caso salimos.
        if (!IsInitialized) return;

        CargarLista();
    }

    /// <summary>Al seleccionar (o dejar de seleccionar) un préstamo, se activa o desactiva "Ver detalle".</summary>
    private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Si hay algo seleccionado (SelectedItem no es null) -> activado
        btnDetalle.IsEnabled = lstPrestamos.SelectedItem != null;
    }

    // ------------------------------------------------------------------
    // NAVEGACIÓN ENTRE VENTANAS
    // ------------------------------------------------------------------

    /// <summary>Botón "Nuevo préstamo": abre el formulario como ventana modal.</summary>
    private void Nuevo_Click(object sender, RoutedEventArgs e)
    {
        // Owner = this -> la ventana principal es la "dueña" del formulario (se centra sobre ella)
        VentanaPrestamo ventana = new VentanaPrestamo { Owner = this };

        // ShowDialog() = modal: bloquea esta ventana hasta cerrar el formulario (apuntes 2.5).
        // Devuelve true si se guardó el préstamo.
        // Si no se guardó (Cancelar, o cerrar con la X), no hay nada que hacer: salimos.
        if (ventana.ShowDialog() != true || ventana.PrestamoCreado == null) return;

        // Para que el préstamo nuevo se vea, ponemos el filtro en "Todos" y recargamos la lista
        rbTodos.IsChecked = true;
        CargarLista();

        // Buscamos el préstamo nuevo en la lista (por su Id) y lo dejamos seleccionado
        int idNuevo = ventana.PrestamoCreado.Id;
        Prestamo? nuevo = lstPrestamos.Items.Cast<Prestamo>().FirstOrDefault(p => p.Id == idNuevo);
        if (nuevo != null)
        {
            lstPrestamos.SelectedItem = nuevo;        // lo selecciona
            lstPrestamos.ScrollIntoView(nuevo);       // [ENUNCIADO] baja la lista hasta que se vea
        }
    }

    /// <summary>Botón "Ver detalle".</summary>
    private void Detalle_Click(object sender, RoutedEventArgs e)
    {
        AbrirDetalle();
    }

    /// <summary>[ENUNCIADO] Doble clic en la lista: también abre el detalle.</summary>
    private void Lista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // [EXTRA] Solo queremos que cuente el doble clic SOBRE una fila, no sobre el hueco
        // vacío ni sobre la barra de desplazamiento.
        // e.OriginalSource = el elemento exacto donde se hizo clic (apuntes 2.6, eventos enrutados).
        DependencyObject? origen = e.OriginalSource as DependencyObject;
        if (origen != null && ItemsControl.ContainerFromElement(lstPrestamos, origen) is ListBoxItem)
        {
            AbrirDetalle();
        }
    }

    /// <summary>Abre la ventana de detalle del préstamo seleccionado (la usan el botón y el doble clic).</summary>
    private void AbrirDetalle()
    {
        // Si no hay un Prestamo seleccionado, no hacemos nada
        if (lstPrestamos.SelectedItem is not Prestamo prestamo) return;

        // Le pasamos el préstamo al constructor del detalle
        VentanaDetalle detalle = new VentanaDetalle(prestamo) { Owner = this };

        // Si el detalle devuelve true (se registró la devolución), recargamos la lista
        if (detalle.ShowDialog() == true)
        {
            CargarLista();
        }
    }

    // ------------------------------------------------------------------
    // CIERRE DE LA APLICACIÓN
    // ------------------------------------------------------------------

    /// <summary>
    /// Se ejecuta cuando se va a cerrar la ventana (evento Closing, apuntes 2.6).
    /// Si hay préstamos vencidos, pregunta antes de salir.
    /// </summary>
    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        int vencidos = App.Prestamos.ObtenerPrestamos().Count(p => p.EstaVencido);
        if (vencidos == 0) return;    // nada vencido: se cierra sin preguntar

        // [EXTRA] Si es 1, el texto va en singular
        string mensaje = vencidos == 1
            ? "Hay 1 préstamo vencido. ¿Quieres salir de todos modos?"
            : "Hay " + vencidos + " préstamos vencidos. ¿Quieres salir de todos modos?";

        // MessageBox con botones Sí/No. El último parámetro (No) es el botón marcado por defecto.
        MessageBoxResult respuesta = MessageBox.Show(
            this, mensaje, "Préstamos vencidos",
            MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

        // e.Cancel = true -> el cierre se cancela y la ventana sigue abierta
        if (respuesta == MessageBoxResult.No)
        {
            e.Cancel = true;
        }
    }
}
