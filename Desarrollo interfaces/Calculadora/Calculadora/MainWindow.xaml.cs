// =====================================================================
//  MainWindow.xaml.cs  ->  LA LÓGICA ("code-behind")
// =====================================================================
//  El archivo MainWindow.xaml dibuja la ventana (el "aspecto").
//  Este archivo dice QUÉ PASA cuando pulsas cada botón (el "cerebro").
//  Los dos forman una única clase gracias a la palabra "partial".
// =====================================================================

using System;                    // Math (para redondear)
using System.Globalization;      // Para convertir texto <-> número sin problemas de idioma
using System.Windows;            // Window, RoutedEventArgs...
using System.Windows.Controls;   // Button

namespace Calculadora;

public partial class MainWindow : Window
{
    // -----------------------------------------------------------------
    //  1) VARIABLES DE ESTADO: lo que la calculadora "recuerda"
    // -----------------------------------------------------------------
    // Imagina que haces 8 + 5 =
    //   - Pulsas 8      -> está en pantalla
    //   - Pulsas +      -> guardamos el 8 en "numeroGuardado" y el "+" en "operadorPendiente"
    //   - Pulsas 5      -> el 5 sale en pantalla
    //   - Pulsas =      -> calculamos numeroGuardado (8) + pantalla (5) = 13

    private double numeroGuardado = 0;       // El primer número de la operación
    private string operadorPendiente = "";   // La operación que falta por hacer ("+", "-", "*", "/")
                                             // Si está vacío ("") significa que no hay ninguna pendiente.

    // Si es true, el PRÓXIMO dígito que pulses empieza un número nuevo
    // (en vez de pegarse al final del que ya está en pantalla).
    // Empieza en true porque la pantalla muestra "0" y queremos que
    // al pulsar un 7 salga "7" y no "07".
    private bool empezarNumeroNuevo = true;

    // -----------------------------------------------------------------
    //  2) CONSTRUCTOR: se ejecuta una vez, al abrir la ventana
    // -----------------------------------------------------------------
    public MainWindow()
    {
        // Esta línea crea los botones y la pantalla que describimos en el XAML.
        // Sin ella la ventana saldría vacía.
        InitializeComponent();
    }

    // -----------------------------------------------------------------
    //  3) MANEJADORES DE EVENTOS: uno por "tipo" de botón
    // -----------------------------------------------------------------
    // En el XAML, cada botón tiene Click="NombreDelMetodo".
    // Cuando pulsas el botón, WPF llama a ese método.
    //   - sender -> el botón que se ha pulsado
    //   - e      -> información del evento (aquí no la necesitamos)
    // Así, con un solo método (Numero_Click) atendemos los 10 dígitos.

    // ---------- Botones 0-9 ----------
    private void Numero_Click(object sender, RoutedEventArgs e)
    {
        // "sender" es un object genérico; lo convertimos a Button para leer su texto.
        Button boton = (Button)sender;
        string digito = boton.Content.ToString();   // Ej.: "7"

        // Si toca empezar número nuevo, O en pantalla solo hay un "0",
        // sustituimos el contenido. Si no, añadimos el dígito al final.
        if (empezarNumeroNuevo || Pantalla.Text == "0")
        {
            Pantalla.Text = digito;
            empezarNumeroNuevo = false;
        }
        else
        {
            Pantalla.Text += digito;   // "12" + "3" -> "123"
        }
    }

    // ---------- Botón . (punto decimal) ----------
    private void Punto_Click(object sender, RoutedEventArgs e)
    {
        if (empezarNumeroNuevo)
        {
            // Si pulsas "." al empezar, escribimos "0." (como las calculadoras reales)
            Pantalla.Text = "0.";
            empezarNumeroNuevo = false;
        }
        else if (!Pantalla.Text.Contains("."))
        {
            // Solo permitimos UN punto por número (evita cosas como "1.2.3")
            Pantalla.Text += ".";
        }
    }

    // ---------- Botones + - * / ----------
    private void Operador_Click(object sender, RoutedEventArgs e)
    {
        Button boton = (Button)sender;
        string nuevoOperador = boton.Content.ToString();   // Ej.: "+"

        // ENCADENAR OPERACIONES: si escribes 2 + 3 + ... al pulsar el segundo "+"
        // primero resolvemos 2 + 3 = 5, y seguimos con ese 5.
        // Solo hacemos esto si ya había un operador pendiente Y el usuario
        // ha escrito un segundo número (si no, sería "2 + +", que no tiene sentido).
        if (operadorPendiente != "" && !empezarNumeroNuevo)
        {
            bool todoBien = ResolverOperacion();
            if (!todoBien) return;   // Hubo error (ej. dividir entre 0): paramos aquí
        }

        // Guardamos lo que hay en pantalla como primer número y recordamos el operador.
        numeroGuardado = LeerPantalla();
        operadorPendiente = nuevoOperador;
        empezarNumeroNuevo = true;   // El siguiente dígito empieza un número nuevo
    }

    // ---------- Botón = ----------
    private void Igual_Click(object sender, RoutedEventArgs e)
    {
        // Si no hay ninguna operación pendiente no hay nada que calcular.
        if (operadorPendiente == "") return;

        ResolverOperacion();

        // Operación terminada: la dejamos "limpia" para la siguiente.
        operadorPendiente = "";
        empezarNumeroNuevo = true;
    }

    // ---------- Botón C ----------
    private void Borrar_Click(object sender, RoutedEventArgs e)
    {
        // Volvemos al estado inicial: todo como al abrir la calculadora.
        Pantalla.Text = "0";
        numeroGuardado = 0;
        operadorPendiente = "";
        empezarNumeroNuevo = true;
    }

    // -----------------------------------------------------------------
    //  4) MÉTODOS AUXILIARES: trozos de código reutilizables
    // -----------------------------------------------------------------

    // Convierte el texto de la pantalla ("12.5") en un número (12.5).
    // Se usa InvariantCulture para que el punto SIEMPRE sea el separador decimal.
    // Si no, en un Windows en español, "12.5" podría interpretarse mal porque
    // allí el separador decimal es la coma.
    private double LeerPantalla()
    {
        // TryParse intenta convertir y devuelve true/false según si ha podido.
        // Si falla (por ejemplo, la pantalla pone "Error"), devolvemos 0.
        bool seHaPodido = double.TryParse(
            Pantalla.Text,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double numero);

        return seHaPodido ? numero : 0;
    }

    // Hace la operación pendiente:  numeroGuardado  (operador)  número de pantalla
    // y muestra el resultado. Devuelve true si todo fue bien, false si hubo error.
    private bool ResolverOperacion()
    {
        double segundoNumero = LeerPantalla();
        double resultado;

        // switch: elige qué hacer según el valor de operadorPendiente.
        switch (operadorPendiente)
        {
            case "+":
                resultado = numeroGuardado + segundoNumero;
                break;
            case "-":
                resultado = numeroGuardado - segundoNumero;
                break;
            case "*":
                resultado = numeroGuardado * segundoNumero;
                break;
            case "/":
                // Dividir entre 0 no se puede: mostramos un error en vez de "infinito".
                if (segundoNumero == 0)
                {
                    MostrarError();
                    return false;
                }
                resultado = numeroGuardado / segundoNumero;
                break;
            default:
                return true;   // No debería pasar, pero así el compilador queda tranquilo
        }

        // Los números con decimales en el ordenador no son exactos:
        // 0.1 + 0.2 daría 0.30000000000000004. Redondeamos a 10 decimales para
        // que se vea 0.3 como esperamos.
        resultado = Math.Round(resultado, 10);

        // Mostramos el resultado (otra vez con InvariantCulture -> punto decimal).
        Pantalla.Text = resultado.ToString(CultureInfo.InvariantCulture);

        // Importante: el resultado pasa a ser el "primer número" por si se encadena otra operación.
        numeroGuardado = resultado;
        return true;
    }

    // Muestra "Error" y deja la calculadora lista para empezar de nuevo.
    private void MostrarError()
    {
        Pantalla.Text = "Error";
        numeroGuardado = 0;
        operadorPendiente = "";
        empezarNumeroNuevo = true;
    }
}