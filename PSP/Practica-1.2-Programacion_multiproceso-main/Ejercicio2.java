import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

/**
 * Ejercicio2 - Lectura y validación de datos de alumnos.
 *
 * El programa lee de la ENTRADA ESTÁNDAR (System.in). Eso le da una gran
 * ventaja: funciona igual si escribes por teclado o si otro programa
 * (Ejecuta_Ejercicio2) le conecta un fichero a la entrada.
 *
 * Formatos de entrada aceptados:
 *   a) Interactivo: el nombre en una línea y la edad en la siguiente.
 *          ANA
 *          33
 *   b) Desde fichero: nombre y edad en la MISMA línea.
 *          ANA 33
 *
 * El bucle termina cuando el nombre es "*".
 * Los errores de validación se muestran por la salida normal (como en el
 * ejemplo del enunciado). La salida de ERROR (System.err) se reserva para
 * problemas reales, por ejemplo que la entrada se acabe sin haber leído "*".
 */
public class Ejercicio2 {

    private static final int EDAD_MIN = 1;
    private static final int EDAD_MAX = 99;
    private static final String FIN = "*";

    public static void main(String[] args) {
        BufferedReader teclado = new BufferedReader(new InputStreamReader(System.in));

        int leidos = 0;                  // alumnos aceptados
        String nombreMayor = null;       // alumno con más edad
        int edadMayor = Integer.MIN_VALUE;
        String nombreMenor = null;       // alumno con menos edad
        int edadMenor = Integer.MAX_VALUE;

        try {
            boolean terminar = false;

            while (!terminar) {
                System.out.println((leidos + 1) + "- Introduce datos de alumnos:");

                // ---------------- LEER EL NOMBRE ----------------
                String linea = null;
                String nombre = null;
                Integer edadEnLinea = null; // edad si venía en la misma línea que el nombre

                while (true) {
                    System.out.println("Escribe un nombre:");
                    linea = teclado.readLine();

                    if (linea == null) { // se acabó la entrada sin encontrar "*"
                        System.err.println("Error: la entrada terminó sin encontrar '" + FIN + "'.");
                        terminar = true;
                        break;
                    }

                    linea = linea.trim();
                    if (linea.length() == 0) { // nombre vacío o en blanco
                        System.out.println("Incorrecto, escríbelo de nuevo:");
                        continue;
                    }

                    // ¿Viene en formato "NOMBRE EDAD"? Miramos si la última palabra es un número.
                    int ultimoEspacio = linea.lastIndexOf(' ');
                    if (ultimoEspacio > 0 && linea.substring(ultimoEspacio + 1).matches("-?\\d+")) {
                        nombre = linea.substring(0, ultimoEspacio).trim();
                        try {
                            edadEnLinea = Integer.parseInt(linea.substring(ultimoEspacio + 1));
                        } catch (NumberFormatException e) {
                            edadEnLinea = Integer.MAX_VALUE; // número enorme: fuera de rango seguro
                        }
                    } else {
                        nombre = linea;
                    }
                    break; // nombre válido
                }

                if (terminar) {
                    break;
                }

                if (nombre.equals(FIN)) { // condición de fin
                    break;
                }

                // ---------------- LEER / VALIDAR LA EDAD ----------------
                int edad = 0;

                if (edadEnLinea != null) {
                    // La edad vino en la misma línea. Si no vale, no hay forma de
                    // "volver a leerla" de esa línea: descartamos el alumno y
                    // pasamos al siguiente.
                    if (edadEnLinea < EDAD_MIN || edadEnLinea > EDAD_MAX) {
                        System.out.println("Incorrecto, debe estar entre " + EDAD_MIN + " y " + EDAD_MAX
                                + " (alumno descartado: " + nombre + ")");
                        continue; // vuelve al principio del while: siguiente alumno
                    }
                    edad = edadEnLinea;
                } else {
                    boolean edadValida = false;
                    while (!edadValida) {
                        System.out.println("Introduce la edad entre " + EDAD_MIN + " y " + EDAD_MAX + ":");
                        String textoEdad = teclado.readLine();

                        if (textoEdad == null) {
                            System.err.println("Error: la entrada terminó mientras se esperaba una edad.");
                            terminar = true;
                            break;
                        }

                        try {
                            edad = Integer.parseInt(textoEdad.trim());
                            if (edad < EDAD_MIN || edad > EDAD_MAX) {
                                System.out.println("Incorrecto, debe estar entre " + EDAD_MIN + " y " + EDAD_MAX);
                            } else {
                                edadValida = true;
                            }
                        } catch (NumberFormatException e) {
                            // Escribieron texto en lugar de un número.
                            System.out.println("Incorrecto, debes escribir un número entero");
                        }
                    }
                    if (terminar) {
                        break;
                    }
                }

                // ---------------- ALUMNO CORRECTO: lo registramos ----------------
                System.out.println("Datos introducidos: " + nombre + ", " + edad);
                leidos++;

                // Con ">" y "<" estrictos, si hay empate se queda el primero que llegó.
                if (edad > edadMayor) {
                    edadMayor = edad;
                    nombreMayor = nombre;
                }
                if (edad < edadMenor) {
                    edadMenor = edad;
                    nombreMenor = nombre;
                }
            }

        } catch (IOException e) {
            System.err.println("Error de lectura: " + e.getMessage());
        }

        // ---------------- RESUMEN FINAL ----------------
        System.out.println("Fin del proceso de lectura...");
        System.out.println("Datos leídos: " + leidos);
        if (leidos > 0) {
            System.out.println("Alumno con más edad: " + nombreMayor);
            System.out.println("Alumno con menos edad: " + nombreMenor);
        } else {
            System.out.println("No se ha leído ningún alumno.");
        }
    }
}
