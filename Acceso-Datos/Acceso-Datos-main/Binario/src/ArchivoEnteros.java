import java.io.IOException;
import java.io.RandomAccessFile;

public class ArchivoEnteros {

    public static final int total_numeros = 10;
    private static final int bytes_Int = 4;

    private final String ruta;

    public ArchivoEnteros(String ruta) {
        this.ruta = ruta;
    }

    //Creo el archivo
    public void crear() throws IOException {
        try (RandomAccessFile raf = new RandomAccessFile(ruta, "rw")) {
            raf.setLength(0); // vacía el archivo si ya existía
            for (int i = 1; i <= total_numeros; i++) {
                raf.writeInt(i * 10); // escribe 4 bytes y avanza el puntero
            }
        }
    }

    //Lee todos los enteros y los muestra por pantalla
    public void mostrar () throws IOException {
        try(RandomAccessFile raf = new RandomAccessFile(ruta, "r")) {
            int posicion = 1;
            while (raf.getFilePointer() < raf.length()) {
                int valor = raf.readInt(); // lee 4 bytes y avanza el puntero
                System.out.println("Posición " + posicion + ": " + valor);
                posicion++;
            }
        }
    }

    //Cambia el número de la posición indicada (1-10)
    public void modificar(int posicion, int nuevoValor) throws IOException {
        try(RandomAccessFile raf = new RandomAccessFile(ruta, "rw")) {
            raf.seek(bytes_Int * (posicion -1)); //salta al número elegido
            raf.writeInt(nuevoValor); //sobreescribe esos 4 bytes
        }
    }
}
