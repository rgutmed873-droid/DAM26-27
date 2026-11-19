using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PrestamosGC.Datos;

/// <summary>
/// Almacén de préstamos de portátiles. NO MODIFICAR.
/// Los datos se guardan en el archivo %LOCALAPPDATA%\PrestamosGC\prestamos.json,
/// así que se conservan al cerrar la aplicación.
/// </summary>
public class GestorPrestamos
{
    private static readonly JsonSerializerOptions Opciones = new JsonSerializerOptions { WriteIndented = true };

    private readonly List<Prestamo> _prestamos;

    /// <summary>Número de portátiles del departamento (P01 a P24).</summary>
    public int TotalPortatiles => 24;

    /// <summary>Ruta completa del archivo de datos. Ábrelo con el Bloc de notas para ver qué se guarda.</summary>
    public string RutaArchivo { get; }

    public GestorPrestamos()
    {
        string carpeta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PrestamosGC");
        Directory.CreateDirectory(carpeta);
        RutaArchivo = Path.Combine(carpeta, "prestamos.json");

        _prestamos = Cargar();

        // La primera vez se crean unos préstamos de ejemplo: uno en préstamo, uno vencido,
        // uno devuelto y uno que empieza hoy.
        if (_prestamos.Count == 0)
        {
            CrearDatosDeEjemplo();
        }
    }

    /// <summary>
    /// Devuelve todos los préstamos: primero los no devueltos, ordenados por fecha prevista de devolución.
    /// Cada llamada devuelve una lista nueva.
    /// </summary>
    public List<Prestamo> ObtenerPrestamos()
    {
        return _prestamos
            .OrderBy(p => p.EstaDevuelto)
            .ThenBy(p => p.FechaDevolucionPrevista)
            .ToList();
    }

    /// <summary>Devuelve el préstamo con ese identificador, o null si no existe.</summary>
    public Prestamo? ObtenerPrestamo(int id)
    {
        return _prestamos.FirstOrDefault(p => p.Id == id);
    }

    /// <summary>Códigos de los portátiles que no están prestados ahora mismo (por ejemplo, "P01").</summary>
    public List<string> PortatilesDisponibles()
    {
        return Enumerable.Range(1, TotalPortatiles)
            .Select(n => "P" + n.ToString("D2"))
            .Where(codigo => !_prestamos.Any(p => p.EstaActivo && p.Portatil == codigo))
            .ToList();
    }

    /// <summary>
    /// Guarda un préstamo nuevo y le asigna su identificador (prestamo.Id).
    /// Lanza InvalidOperationException si el portátil ya está prestado o si el alumno
    /// (mismo correo) ya tiene otro portátil sin devolver.
    /// </summary>
    public void InsertarPrestamo(Prestamo prestamo)
    {
        if (prestamo == null) throw new ArgumentNullException(nameof(prestamo));

        prestamo.Correo = (prestamo.Correo ?? "").Trim();

        if (_prestamos.Any(p => p.EstaActivo && p.Portatil == prestamo.Portatil))
            throw new InvalidOperationException("El portátil " + prestamo.Portatil + " ya está prestado.");

        if (_prestamos.Any(p => p.EstaActivo &&
                string.Equals(p.Correo, prestamo.Correo, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Ese alumno ya tiene un portátil sin devolver.");

        prestamo.Id = _prestamos.Count == 0 ? 1 : _prestamos.Max(p => p.Id) + 1;
        _prestamos.Add(prestamo);
        Guardar();
    }

    /// <summary>
    /// Anota la devolución del préstamo con ese identificador en este momento.
    /// Lanza InvalidOperationException si no existe o si ya se había devuelto.
    /// </summary>
    public void RegistrarDevolucion(int id)
    {
        Prestamo? prestamo = ObtenerPrestamo(id);
        if (prestamo == null)
            throw new InvalidOperationException("No existe ese préstamo.");
        if (prestamo.EstaDevuelto)
            throw new InvalidOperationException("Ese portátil ya se ha devuelto.");

        prestamo.FechaDevolucionReal = DateTime.Now;
        Guardar();
    }

    private void CrearDatosDeEjemplo()
    {
        DateTime hoy = DateTime.Today;

        InsertarPrestamo(new Prestamo
        {
            Nombre = "Lucía", Apellidos = "Pérez Ruiz", Grupo = "2º DAW",
            Correo = "lperez@g.educaand.es", Portatil = "P07", Motivo = "Proyecto",
            IncluyeCargador = true,
            FechaPrestamo = hoy.AddDays(-3), FechaDevolucionPrevista = hoy.AddDays(4)
        });
        InsertarPrestamo(new Prestamo
        {
            Nombre = "Mario", Apellidos = "Gómez Luque", Grupo = "1º DAM",
            Correo = "mgomez@g.educaand.es", Portatil = "P12", Motivo = "Prácticas",
            IncluyeCargador = false,
            FechaPrestamo = hoy.AddDays(-16), FechaDevolucionPrevista = hoy.AddDays(-2)
        });
        InsertarPrestamo(new Prestamo
        {
            Nombre = "Ana", Apellidos = "Ortega Vidal", Grupo = "2º ASIR",
            Correo = "aortega@g.educaand.es", Portatil = "P03", Motivo = "Otro",
            IncluyeCargador = true,
            Observaciones = "Para la presentación del proyecto en la feria de FP.",
            FechaPrestamo = hoy.AddDays(-15), FechaDevolucionPrevista = hoy.AddDays(-8),
            FechaDevolucionReal = hoy.AddDays(-9).AddHours(13)
        });
        InsertarPrestamo(new Prestamo
        {
            Nombre = "Javier", Apellidos = "Ruiz Castro", Grupo = "2º DAM",
            Correo = "jruiz@g.educaand.es", Portatil = "P15", Motivo = "Proyecto",
            IncluyeCargador = true,
            FechaPrestamo = hoy, FechaDevolucionPrevista = hoy.AddDays(14)
        });
    }

    private List<Prestamo> Cargar()
    {
        if (!File.Exists(RutaArchivo)) return new List<Prestamo>();
        try
        {
            string json = File.ReadAllText(RutaArchivo);
            return JsonSerializer.Deserialize<List<Prestamo>>(json) ?? new List<Prestamo>();
        }
        catch (JsonException)
        {
            // Archivo dañado: se empieza de cero
            return new List<Prestamo>();
        }
    }

    private void Guardar()
    {
        File.WriteAllText(RutaArchivo, JsonSerializer.Serialize(_prestamos, Opciones));
    }
}
