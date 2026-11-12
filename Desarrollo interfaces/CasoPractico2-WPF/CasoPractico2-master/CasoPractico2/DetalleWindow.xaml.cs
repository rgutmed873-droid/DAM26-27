using System.Windows;
using System.Windows.Input;

namespace Habitos
{
    // DETALLEWINDOW.XAML.CS: lo que hace la ventana de detalle.
    // Recibe un Habito y enseña sus datos. Se cierra con la tecla ESC.
    public partial class DetalleWindow : Window
    {
        // El constructor recibe el hábito que hay que enseñar.
        // Así la ventana no necesita acceder a la base de datos: ya tiene los datos.
        public DetalleWindow(Habito h)
        {
            InitializeComponent(); // construye los controles del XAML

            // Copiamos cada propiedad del hábito a su TextBlock
            txtId.Text = h.Id.ToString();
            txtNombre.Text = h.Nombre;
            txtCantidad.Text = h.Cantidad.ToString();
            txtFecha.Text = h.Fecha.ToString("dd/MM/yyyy");
        }

        // Se ejecuta al pulsar CUALQUIER tecla mientras esta ventana está activa.
        // e.Key dice cuál ha sido; solo reaccionamos a ESC.
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // Close() cierra esta ventana. Como se abrió con ShowDialog(),
                // el programa vuelve a la línea siguiente de MainWindow
                // y la ventana anterior (la principal) queda otra vez activa.
                Close();
            }
        }
    }
}