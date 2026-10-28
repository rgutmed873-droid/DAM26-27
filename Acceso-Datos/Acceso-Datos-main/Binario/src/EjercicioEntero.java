import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.util.Scanner;

public class EjercicioEntero {
    static void main(String[] args) {

        Scanner sc = new Scanner(System.in);

        ArchivoEnteros archivo = new ArchivoEnteros("src/data/entregaClase.bin");

        try {

            archivo.crear();

            System.out.println("Contenido del archivo: ");
            archivo.mostrar();

            int pos = pedirPosicion(sc);

            System.out.println("Introduce un nuevo valor: ");
            int nuevoValor = sc.nextInt();

            archivo.modificar(pos, nuevoValor);

            System.out.println("Contenido actualizado del archivo: ");
            archivo.mostrar();

        } catch (IOException e) {
            System.out.println("Error con el archivo: " + e.getMessage());
        }
    }


    private static int pedirPosicion(Scanner sc) {
        int pos;
        do {
            System.out.println("Elige la posición del número que quieres modificar (1-" + ArchivoEnteros.total_numeros + "): ");
            pos = sc.nextInt();

        } while (pos < 1 || pos > ArchivoEnteros.total_numeros);
        return pos;
    }

}
