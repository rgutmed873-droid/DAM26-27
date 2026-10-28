import java.io.File;
import java.io.IOException;

/**
 * Ejecuta_Ejercicio2 - PROGRAMA PADRE que lanza Ejercicio2 una vez por fichero.
 *
 * Uso:   java Ejecuta_Ejercicio2 Fichero1.txt Fichero2.txt ...
 *
 * Para cada fichero de entrada F que exista:
 *   - lanza un proceso "java Ejercicio2"
 *   - le CONECTA el fichero F como entrada      (como si lo escribieras a teclado)
 *   - guarda su salida normal en el fichero SF   (S + nombre)
 *   - guarda su salida de error en el fichero EF (E + nombre)
 *
 * Esto equivale en una terminal a:
 *     java Ejercicio2 < F > SF 2> EF
 * pero hecho desde Java con ProcessBuilder y sus "redirecciones".
 */
public class Ejecuta_Ejercicio2 {

    public static void main(String[] args) {
        // 1) Comprobar que se ha recibido al menos un argumento.
        if (args.length == 0) {
            System.out.println("Error: debes indicar al menos un fichero de entrada.");
            System.out.println("Uso: java Ejecuta_Ejercicio2 <fichero1> [<fichero2> ...]");
            return; // finaliza el programa
        }

        // Cómo lanzar "java Ejercicio2": mismo java y mismo classpath que este programa.
        String java = System.getProperty("java.home") + "/bin/java";
        String classpath = System.getProperty("java.class.path");

        int ejecuciones = 0; // veces que realmente se ha ejecutado Ejercicio2

        for (String nombreFichero : args) {
            File entrada = new File(nombreFichero);

            // 2) Si el fichero no existe, avisamos y seguimos con el siguiente.
            if (!entrada.isFile()) {
                System.out.println("El fichero '" + nombreFichero + "' no existe. Se omite.");
                continue;
            }

            // 3) Construir los nombres de salida: S<nombre> y E<nombre>,
            //    en la misma carpeta que el fichero de entrada.
            File salida = new File(entrada.getParentFile(), "S" + entrada.getName());
            File error = new File(entrada.getParentFile(), "E" + entrada.getName());

            // 4) Preparar el proceso hijo con sus tres redirecciones.
            ProcessBuilder pb = new ProcessBuilder(java, "-cp", classpath, "Ejercicio2");
            pb.redirectInput(entrada);   // lo que el hijo lee, sale del fichero
            pb.redirectOutput(salida);   // lo que el hijo escribe, va al fichero S...
            pb.redirectError(error);     // sus errores van al fichero E... (se crea aunque quede vacío)

            try {
                Process hijo = pb.start();
                int codigo = hijo.waitFor(); // esperamos a que acabe antes de lanzar el siguiente
                ejecuciones++;
                System.out.println("Ejecutado Ejercicio2 con '" + nombreFichero + "' (código de salida "
                        + codigo + ") -> " + salida.getName() + ", " + error.getName());
            } catch (IOException e) {
                System.out.println("No se pudo ejecutar Ejercicio2 con '" + nombreFichero + "': " + e.getMessage());
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                System.out.println("La ejecución con '" + nombreFichero + "' fue interrumpida.");
                break;
            }
        }

        // 5) Mensaje final con el número de ejecuciones.
        System.out.println("Ejercicio2 se ha ejecutado " + ejecuciones + " vez/veces.");
        System.out.println("Fin del proceso.");
    }
}
