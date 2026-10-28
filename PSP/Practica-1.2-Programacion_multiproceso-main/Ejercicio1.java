import java.util.Scanner;

/**
 * Ejercicio1 - PROCESO HIJO.
 *
 * Este programa no se ejecuta solo: lo lanza el padre (Ejecuta_Ejercicio1).
 * Su trabajo es multiplicar, posición a posición, dos trozos de array.
 *
 * PROTOCOLO DE COMUNICACIÓN (lo que el padre le manda por la entrada estándar):
 *   línea 1: n            -> cuántos números tiene cada trozo
 *   línea 2: a1 a2 ... an -> trozo del primer array
 *   línea 3: b1 b2 ... bn -> trozo del segundo array
 *
 * RESPUESTA (lo que el hijo escribe por la salida estándar):
 *   una sola línea: a1*b1 a2*b2 ... an*bn
 *
 * Si algo falla, el hijo escribe el motivo por la salida de ERROR (System.err)
 * y termina con un código de salida distinto de 0.
 */
public class Ejercicio1 {

    public static void main(String[] args) {
        // Scanner sobre System.in: aquí llegan los datos que escribe el padre.
        Scanner entrada = new Scanner(System.in);

        try {
            // 1) Leemos cuántos elementos tiene cada trozo.
            int n = entrada.nextInt();

            // 2) Leemos los dos trozos de array.
            int[] a = new int[n];
            int[] b = new int[n];
            for (int i = 0; i < n; i++) {
                a[i] = entrada.nextInt();
            }
            for (int i = 0; i < n; i++) {
                b[i] = entrada.nextInt();
            }

            // 3) Multiplicamos posición a posición y construimos la respuesta.
            StringBuilder resultado = new StringBuilder();
            for (int i = 0; i < n; i++) {
                if (i > 0) {
                    resultado.append(' '); // separador entre números
                }
                resultado.append(a[i] * b[i]);
            }

            // 4) Devolvemos el resultado al padre por la salida estándar.
            System.out.println(resultado);

        } catch (Exception e) {
            // Cualquier problema (faltan datos, no es un número...) va por stderr.
            System.err.println("Ejercicio1 (hijo): datos de entrada incorrectos -> " + e);
            System.exit(1); // código de salida 1 = terminó con error
        }
    }
}
