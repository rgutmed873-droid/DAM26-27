namespace Habitos
{
    // Una fila de la tabla "habitos".
    // La tabla de la ventana (DataGrid) muestra estas propiedades por su nombre.
    public class Habito
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
