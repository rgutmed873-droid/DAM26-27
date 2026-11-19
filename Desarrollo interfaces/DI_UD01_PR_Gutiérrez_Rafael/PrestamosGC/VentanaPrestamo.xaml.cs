using System.Net.Mail;          // MailAddress (para comprobar el formato del correo)
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;      // Brush (colores)
using PrestamosGC.Datos;

namespace PrestamosGC;

// ETIQUETAS: [ENUNCIADO] = lo pide el PDF de la práctica pero no está en los apuntes
//            [EXTRA]     = lo he añadido yo por mi cuenta
//            Sin etiqueta = está en los apuntes

/// <summary>
/// FORMULARIO DE PRÉSTAMO NUEVO. Se abre como ventana modal desde la ventana principal.
/// Si se guarda, DialogResult vale true y el préstamo queda en la propiedad PrestamoCreado.
/// </summary>
public partial class VentanaPrestamo : Window
{
    // Constantes: valores fijos que usamos varias veces (así, si cambian, se cambian en un solo sitio)
    private const string DominioCorreo = "@g.educaand.es";
    private const int MaxDiasPrestamo = 14;

    // [EXTRA] Diccionario: relaciona cada campo con el TextBlock donde sale SU error.
    // Sirve para poder ocultar el error de un campo en cuanto el usuario lo corrige (ejercicio 5f)
    // con un único método. Los diccionarios no salen en los apuntes.
    private readonly Dictionary<Control, TextBlock> _errores;

    /// <summary>
    /// Aquí queda el préstamo guardado, para que la ventana principal lo pueda leer (ejercicio 5f).
    /// "private set": solo se puede asignar desde dentro de esta clase. null = no se ha guardado nada.
    /// </summary>
    public Prestamo? PrestamoCreado { get; private set; }

    // CONSTRUCTOR: se ejecuta al crear la ventana
    public VentanaPrestamo()
    {
        InitializeComponent();   // carga el XAML y crea los controles

        // Rellenamos el diccionario campo -> su mensaje de error
        _errores = new Dictionary<Control, TextBlock>
        {
            { txtNombre, errNombre },
            { txtApellidos, errApellidos },
            { cmbGrupo, errGrupo },
            { txtCorreo, errCorreo },
            { cmbPortatil, errPortatil },
            { rbProyecto, errMotivo },      // los 3 RadioButton comparten el mismo error
            { rbPracticas, errMotivo },
            { rbOtro, errMotivo },
            { dpPrestamo, errFechaPrestamo },
            { dpDevolucion, errFechaDevolucion },
            { txtObservaciones, errObservaciones },
            { chkNormas, errNormas }
        };

        // EJERCICIO 5a: preparar el formulario al abrirlo
        // 1) El ComboBox de portátiles se rellena con los que están libres (ItemsSource, apuntes 2.5)
        cmbPortatil.ItemsSource = App.Prestamos.PortatilesDisponibles();
        // 2) [ENUNCIADO] DisplayDateStart = primer día que se puede elegir en el calendario: hoy
        dpPrestamo.DisplayDateStart = DateTime.Today;
        // 3) La fecha de préstamo empieza en hoy.
        //    Al asignarla se lanza el evento SelectedDateChanged -> FechaPrestamo_SelectedDateChanged
        dpPrestamo.SelectedDate = DateTime.Today;
    }

    // ------------------------------------------------------------------
    // EJERCICIOS 5b y 5c: eventos que encadenan unos controles con otros
    // ------------------------------------------------------------------

    /// <summary>
    /// EJERCICIO 5b: la fecha de devolución solo puede ir entre el día siguiente
    /// y 14 días después de la fecha de préstamo.
    /// Se ejecuta cada vez que cambia la fecha de préstamo.
    /// </summary>
    private void FechaPrestamo_SelectedDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        OcultarError(dpPrestamo, errFechaPrestamo);   // si había error en esta fecha, se quita

        // SelectedDate es DateTime? (puede ser null = sin fecha), apuntes 2.5
        DateTime? prestamo = dpPrestamo.SelectedDate;

        // Si no hay fecha de préstamo, quitamos los límites de la otra fecha y terminamos
        if (prestamo == null)
        {
            dpDevolucion.DisplayDateStart = null;
            dpDevolucion.DisplayDateEnd = null;
            return;
        }

        // Calculamos el margen permitido: mínimo = día siguiente, máximo = 14 días después
        DateTime minimo = prestamo.Value.Date.AddDays(1);
        DateTime maximo = prestamo.Value.Date.AddDays(MaxDiasPrestamo);

        // Si la devolución que ya estaba elegida queda fuera del nuevo margen, se borra
        DateTime? devolucion = dpDevolucion.SelectedDate;
        if (devolucion != null && (devolucion.Value.Date < minimo || devolucion.Value.Date > maximo))
        {
            dpDevolucion.SelectedDate = null;
        }

        // Ponemos los nuevos límites del calendario de devolución.
        // [EXTRA] Primero se quitan los antiguos para que nunca se queden cruzados
        // (inicio mayor que fin daría error).
        dpDevolucion.DisplayDateStart = null;
        dpDevolucion.DisplayDateEnd = null;
        dpDevolucion.DisplayDateStart = minimo;
        dpDevolucion.DisplayDateEnd = maximo;
    }

    /// <summary>
    /// EJERCICIO 5c: contador de caracteres de las observaciones.
    /// Se ejecuta cada vez que cambia el texto.
    /// </summary>
    private void Observaciones_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Ejemplo de resultado: "37/200"
        txtContador.Text = txtObservaciones.Text.Length + "/" + txtObservaciones.MaxLength;
        OcultarError(txtObservaciones, errObservaciones);
    }

    /// <summary>
    /// EJERCICIO 5c: si el motivo elegido es «Otro», las observaciones pasan a ser obligatorias.
    /// Se ejecuta cuando se marca cualquiera de los 3 RadioButton del motivo.
    /// </summary>
    private void Motivo_Checked(object sender, RoutedEventArgs e)
    {
        OcultarError(rbProyecto, errMotivo);   // ya ha elegido motivo: se quita el error

        bool esOtro = rbOtro.IsChecked == true;
        // Cambiamos el texto de la etiqueta según el motivo
        lblObservaciones.Text = esOtro ? "Observaciones (obligatorias)" : "Observaciones";

        // Si ya no es «Otro», el error de las observaciones deja de tener sentido
        if (!esOtro)
        {
            OcultarError(txtObservaciones, errObservaciones);
        }
    }

    // ------------------------------------------------------------------
    // EJERCICIO 5f: el mensaje de error desaparece en cuanto se corrige el campo
    // ------------------------------------------------------------------

    /// <summary>
    /// [EXTRA] UN SOLO manejador para muchos eventos distintos (TextChanged, SelectionChanged,
    /// SelectedDateChanged y Checked). Funciona porque todos esos "e" heredan de RoutedEventArgs.
    /// "sender" = el control que ha cambiado; con el diccionario buscamos su mensaje de error.
    /// (Los apuntes enseñan un manejador para varios botones; esta variante es más avanzada.)
    /// </summary>
    private void CampoCambiado(object sender, RoutedEventArgs e)
    {
        // Mientras se construye la ventana el diccionario aún no existe: salimos
        if (_errores == null) return;

        // TryGetValue: "busca este control en el diccionario; si está, dame su TextBlock de error"
        if (sender is Control campo && _errores.TryGetValue(campo, out TextBlock? error))
        {
            OcultarError(campo, error);
        }
    }

    /// <summary>Enseña el mensaje de error bajo un campo y le pone el borde rojo.</summary>
    private void MostrarError(Control campo, TextBlock error, string mensaje)
    {
        error.Text = mensaje;
        error.Visibility = Visibility.Visible;   // Visible = se ve (antes estaba Collapsed = oculto)

        // [EXTRA] Borde rojo en las cajas (como en la figura 2).
        // FindResource busca el pincel "ColorError" que está definido en App.xaml.
        if (campo is TextBox or ComboBox or DatePicker)
        {
            campo.BorderBrush = (Brush)FindResource("ColorError");
        }
    }

    /// <summary>Oculta el mensaje de error de un campo y le quita el borde rojo.</summary>
    private void OcultarError(Control campo, TextBlock error)
    {
        error.Text = "";
        error.Visibility = Visibility.Collapsed;
        // [EXTRA] ClearValue = "olvida el valor que puse yo" -> vuelve al borde normal
        campo.ClearValue(Control.BorderBrushProperty);
    }

    // ------------------------------------------------------------------
    // EJERCICIOS 5d y 5e: validar y registrar el préstamo
    // ------------------------------------------------------------------

    /// <summary>
    /// Botón «Registrar préstamo»: valida el formulario y, si todo está bien, guarda el préstamo.
    /// </summary>
    private void Registrar_Click(object sender, RoutedEventArgs e)
    {
        // Antes de empezar, ocultamos el error general que pudiera haber de un intento anterior
        txtErrorGeneral.Visibility = Visibility.Collapsed;

        // Si hay algún campo mal, Validar() ya ha puesto los mensajes: no seguimos
        if (!Validar()) return;

        // Todo correcto: creamos el objeto Prestamo con los datos del formulario
        Prestamo prestamo = new Prestamo
        {
            Nombre = txtNombre.Text.Trim(),          // Trim() quita espacios sobrantes
            Apellidos = txtApellidos.Text.Trim(),

            // OJO (lo avisa el enunciado): el ComboBox del grupo tiene ComboBoxItem escritos en el
            // XAML, así que SelectedItem es un ComboBoxItem y el texto está en su Content.
            // El "!" significa "sé que no es null" (ya lo comprobó Validar).
            Grupo = ((ComboBoxItem)cmbGrupo.SelectedItem!).Content?.ToString() ?? "",

            Correo = txtCorreo.Text.Trim(),

            // El ComboBox de portátiles se rellenó con cadenas (string), así que
            // SelectedItem ya es directamente el texto, por ejemplo "P07".
            Portatil = (string)cmbPortatil.SelectedItem!,

            Motivo = MotivoSeleccionado() ?? "",
            IncluyeCargador = chkCargador.IsChecked == true,
            Observaciones = txtObservaciones.Text.Trim(),

            // .Value = el valor real del DateTime? (que ya sabemos que no es null)
            // .Date = solo la fecha, sin la hora
            FechaPrestamo = dpPrestamo.SelectedDate!.Value.Date,
            FechaDevolucionPrevista = dpDevolucion.SelectedDate!.Value.Date
        };

        // EJERCICIO 5e: InsertarPrestamo puede lanzar una excepción, así que va dentro de un try/catch
        try
        {
            App.Prestamos.InsertarPrestamo(prestamo);
        }
        catch (InvalidOperationException ex)
        {
            // El portátil ya está prestado, o el alumno ya tiene otro sin devolver.
            // Mostramos el mensaje de la excepción en txtErrorGeneral y NO cerramos la ventana.
            MostrarErrorGeneral(ex.Message);
            return;
        }
        // [EXTRA] Por si falla al escribir el archivo JSON en el disco
        // ("when" = este catch solo atrapa esos dos tipos de error)
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MostrarErrorGeneral("No se ha podido guardar el préstamo: " + ex.Message);
            return;
        }

        // Se guardó bien: dejamos el préstamo en la propiedad pública y cerramos con "true".
        // DialogResult = true cierra la ventana modal y hace que ShowDialog() devuelva true.
        PrestamoCreado = prestamo;
        DialogResult = true;
    }

    /// <summary>Muestra un mensaje en el error general (encima de los botones).</summary>
    private void MostrarErrorGeneral(string mensaje)
    {
        txtErrorGeneral.Text = mensaje;
        txtErrorGeneral.Visibility = Visibility.Visible;
    }

    /// <summary>Devuelve el motivo elegido ("Proyecto", "Prácticas" u "Otro"), o null si no hay ninguno.</summary>
    private string? MotivoSeleccionado()
    {
        if (rbProyecto.IsChecked == true) return "Proyecto";
        if (rbPracticas.IsChecked == true) return "Prácticas";
        if (rbOtro.IsChecked == true) return "Otro";
        return null;
    }

    /// <summary>
    /// EJERCICIO 5d: comprueba TODAS las reglas de la tabla del enunciado.
    /// Por cada campo incorrecto muestra su mensaje, y al final lleva el foco al primero.
    /// Devuelve true si todo es correcto y false si hay algún error.
    /// </summary>
    private bool Validar()
    {
        // Aquí guardaremos el PRIMER campo incorrecto (para ponerle el foco al final)
        Control? primero = null;

        // [EXTRA] Función pequeña dentro del método ("función local"): muestra el error
        // y apunta el campo si es el primero que falla. "??=" = "asigna solo si todavía es null".
        void Error(Control campo, TextBlock error, string mensaje)
        {
            MostrarError(campo, error, mensaje);
            primero ??= campo;
        }

        // ----- Nombre y apellidos: obligatorios -----
        // IsNullOrWhiteSpace = true si está vacío o solo tiene espacios
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
            Error(txtNombre, errNombre, "Campo obligatorio.");
        if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            Error(txtApellidos, errApellidos, "Campo obligatorio.");

        // ----- Grupo: hay que elegir uno (SelectedItem null = no hay nada elegido) -----
        if (cmbGrupo.SelectedItem == null)
            Error(cmbGrupo, errGrupo, "Elige un grupo.");

        // ----- Correo: formato válido Y terminado en @g.educaand.es -----
        string correo = txtCorreo.Text.Trim();
        // [ENUNCIADO] MailAddress.TryCreate (es la pista del enunciado): intenta crear una dirección
        // de correo y devuelve false si el formato no es válido.
        bool correoValido =
            MailAddress.TryCreate(correo, out MailAddress? direccion) &&                     // 1) formato válido
            string.Equals(direccion.Address, correo, StringComparison.OrdinalIgnoreCase) &&  // 2) sin nombre extra delante
            correo.EndsWith(DominioCorreo, StringComparison.OrdinalIgnoreCase) &&            // 3) acaba en @g.educaand.es
            correo.Length > DominioCorreo.Length;                                            // 4) hay algo antes de la @
        if (!correoValido)
            Error(txtCorreo, errCorreo, "Usa el correo del centro (" + DominioCorreo + ").");

        // ----- Portátil: hay que elegir uno -----
        if (cmbPortatil.SelectedItem == null)
            Error(cmbPortatil, errPortatil, "Elige un portátil.");

        // ----- Motivo: hay que marcar un RadioButton -----
        string? motivo = MotivoSeleccionado();
        if (motivo == null)
            Error(rbProyecto, errMotivo, "Elige un motivo.");   // el foco irá al primer RadioButton

        // ----- Fecha de préstamo: obligatoria y no anterior a hoy -----
        DateTime? fechaPrestamo = dpPrestamo.SelectedDate;
        if (fechaPrestamo == null || fechaPrestamo.Value.Date < DateTime.Today)
            Error(dpPrestamo, errFechaPrestamo, "La fecha no puede ser anterior a hoy.");

        // ----- Fecha de devolución: entre 1 y 14 días después de la de préstamo -----
        DateTime? fechaDevolucion = dpDevolucion.SelectedDate;
        if (fechaDevolucion == null)
        {
            Error(dpDevolucion, errFechaDevolucion, "El préstamo dura entre 1 y " + MaxDiasPrestamo + " días.");
        }
        else if (fechaPrestamo != null)
        {
            // Restar dos fechas da un TimeSpan; .Days = número de días de diferencia
            int dias = (fechaDevolucion.Value.Date - fechaPrestamo.Value.Date).Days;
            if (dias < 1 || dias > MaxDiasPrestamo)
                Error(dpDevolucion, errFechaDevolucion, "El préstamo dura entre 1 y " + MaxDiasPrestamo + " días.");
        }

        // ----- Observaciones: hasta 200 caracteres (ya lo garantiza MaxLength) y obligatorias si el motivo es «Otro» -----
        if (motivo == "Otro" && string.IsNullOrWhiteSpace(txtObservaciones.Text))
            Error(txtObservaciones, errObservaciones, "Explica el motivo del préstamo.");

        // ----- Normas de uso: la casilla tiene que estar marcada -----
        if (chkNormas.IsChecked != true)
            Error(chkNormas, errNormas, "El alumno debe aceptar las normas.");

        // ----- Resultado -----
        // Si hubo algún error, "primero" tiene el primer campo incorrecto: le damos el foco
        if (primero != null)
        {
            primero.Focus();
            return false;
        }

        return true;
    }
}
