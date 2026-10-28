import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.util.Arrays;
import java.util.Random;

/**
 * Ejecuta_Ejercicio1 - PROCESO PADRE.
 *
 * Qué hace:
 *   1. Crea dos arrays de 10 enteros aleatorios.
 *   2. Parte cada array en dos mitades (posiciones 0-4 y posiciones 5-9).
 *   3. Lanza DOS procesos hijos (clase Ejercicio1), uno por mitad, y les envía
 *      las mitades correspondientes de los dos arrays.
 *   4. Recoge el resultado de cada hijo y compone el array final.
 *   5. Muestra todo por consola.
 *
 * Idea clave: padre e hijo son programas distintos y SOLO se comunican por
 * "tuberías" (streams):
 *     padre --(escribe)--> entrada estándar del hijo
 *     hijo  --(escribe)--> salida estándar del hijo --(lee)--> padre
 */
public class Ejecuta_Ejercicio1 {

    private static final int TAMANO = 10;     // dimensión de los arrays
    private static final int NUM_HIJOS = 2;   // número de procesos hijos

    public static void main(String[] args) {
        // ---------- 1. Inicializar los arrays con números aleatorios ----------
        Random aleatorio = new Random();
        int[] arrayA = new int[TAMANO];
        int[] arrayB = new int[TAMANO];
        for (int i = 0; i < TAMANO; i++) {
            arrayA[i] = aleatorio.nextInt(100) + 1; // números del 1 al 100
            arrayB[i] = aleatorio.nextInt(100) + 1;
        }
        System.out.println("Array A:         " + Arrays.toString(arrayA));
        System.out.println("Array B:         " + Arrays.toString(arrayB));

        int[] resultado = new int[TAMANO]; // aquí se irá guardando la multiplicación
        int mitad = TAMANO / NUM_HIJOS;    // 10 / 2 = 5 elementos por hijo

        // Necesitamos saber cómo lanzar "java Ejercicio1". Usamos el mismo java
        // y el mismo classpath con el que se está ejecutando este programa.
        String java = System.getProperty("java.home") + "/bin/java";
        String classpath = System.getProperty("java.class.path");

        Process[] hijos = new Process[NUM_HIJOS];

        try {
            // ---------- 2. Lanzar los hijos y enviarles sus datos ----------
            for (int h = 0; h < NUM_HIJOS; h++) {
                int desde = h * mitad;     // primera posición de este hijo
                int hasta = desde + mitad; // última posición (excluida)

                // ProcessBuilder prepara el comando: java -cp <classpath> Ejercicio1
                ProcessBuilder pb = new ProcessBuilder(java, "-cp", classpath, "Ejercicio1");
                // Si el hijo escribe errores, que salgan en NUESTRA consola.
                pb.redirectError(ProcessBuilder.Redirect.INHERIT);

                hijos[h] = pb.start(); // ¡aquí nace el proceso hijo!

                // Escribimos en la ENTRADA ESTÁNDAR del hijo (getOutputStream).
                // Recuerda: lo que el padre "saca" es lo que el hijo "mete".
                PrintWriter haciaHijo = new PrintWriter(hijos[h].getOutputStream());
                haciaHijo.println(mitad);
                haciaHijo.println(unirNumeros(Arrays.copyOfRange(arrayA, desde, hasta)));
                haciaHijo.println(unirNumeros(Arrays.copyOfRange(arrayB, desde, hasta)));
                haciaHijo.close(); // cerrar = avisar al hijo de que no hay más datos
            }

            // ---------- 3. Recoger los resultados de los hijos ----------
            for (int h = 0; h < NUM_HIJOS; h++) {
                int desde = h * mitad;

                // Leemos la SALIDA ESTÁNDAR del hijo (getInputStream).
                BufferedReader delHijo = new BufferedReader(
                        new InputStreamReader(hijos[h].getInputStream()));
                String linea = delHijo.readLine();
                delHijo.close();

                int codigo = hijos[h].waitFor(); // esperamos a que el hijo termine
                if (codigo != 0 || linea == null) {
                    System.err.println("El hijo " + (h + 1) + " terminó con error (código " + codigo + ").");
                    return;
                }

                // Convertimos "12 30 44 ..." en números y los colocamos en su sitio.
                String[] partes = linea.trim().split("\\s+");
                for (int i = 0; i < partes.length; i++) {
                    resultado[desde + i] = Integer.parseInt(partes[i]);
                }
                System.out.println("Hijo " + (h + 1) + " devolvió:  " + linea);
            }

        } catch (IOException e) {
            System.err.println("No se pudo lanzar o comunicar con un proceso hijo: " + e.getMessage());
            return;
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt(); // buena práctica al capturar esta excepción
            System.err.println("Se interrumpió la espera del proceso hijo.");
            return;
        }

        // ---------- 4. Mostrar el resultado final ----------
        System.out.println("Resultado A x B: " + Arrays.toString(resultado));

        // Comprobación extra: calculamos lo mismo sin hijos y comparamos.
        int[] esperado = new int[TAMANO];
        for (int i = 0; i < TAMANO; i++) {
            esperado[i] = arrayA[i] * arrayB[i];
        }
        System.out.println("Comprobación:    " + (Arrays.equals(resultado, esperado) ? "CORRECTO" : "ERROR"));
    }

    /** Convierte {3, 5, 7} en el texto "3 5 7" para enviarlo al hijo. */
    private static String unirNumeros(int[] numeros) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < numeros.length; i++) {
            if (i > 0) {
                sb.append(' ');
            }
            sb.append(numeros[i]);
        }
        return sb.toString();
    }
}
