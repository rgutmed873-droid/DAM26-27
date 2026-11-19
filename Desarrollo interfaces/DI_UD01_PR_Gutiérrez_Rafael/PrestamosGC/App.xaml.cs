using System.Windows;
using PrestamosGC.Datos;

namespace PrestamosGC;

/// <summary>
/// Punto de entrada de la aplicación. La ventana inicial se indica en App.xaml (StartupUri).
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Almacén de préstamos, compartido por todas las ventanas.
    /// Se usa así: App.Prestamos.ObtenerPrestamos()
    /// </summary>
    public static GestorPrestamos Prestamos { get; } = new GestorPrestamos();
}
