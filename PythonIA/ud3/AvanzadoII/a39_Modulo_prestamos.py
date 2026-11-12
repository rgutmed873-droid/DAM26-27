#Actividad 39

# principal.py

# Importación en una sola línea
from prestamos import prestar, PrestamoError, SinEjemplaresError, LibroDesconocidoError, DiasError

def a_dias(texto):
    """Convierte texto a entero. Si falla, emite un aviso y re-lanza el error."""
    try:
        return int(texto)
    except ValueError:
        print(f"(aviso) '{texto}' no es un número de días")
        raise

# Datos del enunciado
catalogo = {"Dune": 1, "Dune (copia)": 1, "Ensayo": 0, "Quijote": 2}
solicitudes = [
    ("Dune", "14"), 
    ("Dune", "7"), 
    ("Dune", "5"), 
    ("Quijote", "45"), 
    ("Quijote", "x"), 
    ("Ensayo", "10"), 
    ("Hamlet", "3")
]

# Procesamiento de solicitudes
for titulo, texto_dias in solicitudes:
    print(f"\n---> Solicitud: {titulo} por {texto_dias} días")
    
    # Try exterior: recoge rechazos no recuperados (incluye fallos en la copia)
    try:
        # Try interior: intenta la conversión y el préstamo con recuperación
        try:
            dias = a_dias(texto_dias)
            quedan = prestar(catalogo, titulo, dias)
        except SinEjemplaresError:
            copia = titulo + " (copia)"
            print(f"Sin ejemplares de '{titulo}'. Intentando recuperación con '{copia}'...")
            quedan = prestar(catalogo, copia, dias)
            print(f"Préstamo concedido (copia). Quedan {quedan} ejemplares de '{copia}'.")
        except ValueError as error:
            print(f"Rechazado en interior ({type(error).__name__}): {error}")
        else:
            print(f"Préstamo concedido. Quedan {quedan} ejemplares de '{titulo}'.")
            
    except PrestamoError as error:
        print(f"Rechazado en exterior ({type(error).__name__}): {error}")