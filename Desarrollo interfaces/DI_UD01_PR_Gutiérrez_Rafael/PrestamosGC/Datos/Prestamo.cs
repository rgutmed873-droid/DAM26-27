using System;
using System.Text.Json.Serialization;

namespace PrestamosGC.Datos;

/// <summary>
/// Préstamo de un portátil del departamento a un alumno. NO MODIFICAR.
/// </summary>
public class Prestamo
{
    /// <summary>Identificador del préstamo. Lo asigna GestorPrestamos al insertarlo.</summary>
    public int Id { get; set; }

    public string Nombre { get; set; } = "";

    public string Apellidos { get; set; } = "";

    /// <summary>Grupo del alumno: 1º DAM, 2º DAM, 1º DAW, 2º DAW, 1º ASIR o 2º ASIR.</summary>
    public string Grupo { get; set; } = "";

    /// <summary>Correo del alumno en el centro (termina en @g.educaand.es).</summary>
    public string Correo { get; set; } = "";

    /// <summary>Código del portátil, de P01 a P24.</summary>
    public string Portatil { get; set; } = "";

    /// <summary>Proyecto, Prácticas u Otro.</summary>
    public string Motivo { get; set; } = "";

    public bool IncluyeCargador { get; set; }

    public string Observaciones { get; set; } = "";

    public DateTime FechaPrestamo { get; set; } = DateTime.Today;

    public DateTime FechaDevolucionPrevista { get; set; } = DateTime.Today;

    /// <summary>Momento en que se devolvió el portátil, o null si todavía no se ha devuelto.</summary>
    public DateTime? FechaDevolucionReal { get; set; }

    // ----- Propiedades calculadas (de solo lectura, pensadas para usarlas con Binding) -----

    /// <summary>Nombre y apellidos en una sola cadena.</summary>
    [JsonIgnore]
    public string NombreCompleto => (Nombre + " " + Apellidos).Trim();

    [JsonIgnore]
    public bool EstaDevuelto => FechaDevolucionReal.HasValue;

    /// <summary>True mientras el portátil no se ha devuelto.</summary>
    [JsonIgnore]
    public bool EstaActivo => !EstaDevuelto;

    /// <summary>True si no se ha devuelto y ya ha pasado la fecha prevista.</summary>
    [JsonIgnore]
    public bool EstaVencido => EstaActivo && FechaDevolucionPrevista.Date < DateTime.Today;

    /// <summary>«En préstamo», «Vencido» o «Devuelto».</summary>
    [JsonIgnore]
    public string Estado => EstaDevuelto ? "Devuelto" : (EstaVencido ? "Vencido" : "En préstamo");

    /// <summary>Días que faltan hasta la fecha prevista (negativo si ya ha pasado).</summary>
    [JsonIgnore]
    public int DiasRestantes => (FechaDevolucionPrevista.Date - DateTime.Today).Days;

    /// <summary>Días entre la fecha de préstamo y la fecha prevista de devolución.</summary>
    [JsonIgnore]
    public int DuracionDias => (FechaDevolucionPrevista.Date - FechaPrestamo.Date).Days;

    /// <summary>«Sí» o «No».</summary>
    [JsonIgnore]
    public string CargadorTexto => IncluyeCargador ? "Sí" : "No";

    /// <summary>Texto con la situación del plazo: «Quedan 4 días», «Vence hoy», «Vencido hace 2 días»...</summary>
    [JsonIgnore]
    public string Plazo
    {
        get
        {
            if (EstaDevuelto) return "Devuelto el " + FechaDevolucionReal!.Value.ToString("dd/MM/yyyy");
            int dias = DiasRestantes;
            if (dias > 1) return "Quedan " + dias + " días";
            if (dias == 1) return "Queda 1 día";
            if (dias == 0) return "Vence hoy";
            if (dias == -1) return "Vencido hace 1 día";
            return "Vencido hace " + (-dias) + " días";
        }
    }

    /// <summary>Línea de resumen para mostrar el préstamo en una lista.</summary>
    [JsonIgnore]
    public string Resumen =>
        Portatil + " · " + NombreCompleto + " (" + Grupo + ") · hasta " +
        FechaDevolucionPrevista.ToString("dd/MM/yyyy") + " · " + Estado;
}
