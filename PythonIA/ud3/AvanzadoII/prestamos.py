#Actividad 39

# Archivo de python de de prestamos

# 1. Jerarquía de excepciones
class PrestamoError(Exception):
    pass

class SinEjemplaresError(PrestamoError):
    pass

class LibroDesconocidoError(PrestamoError):
    pass

class DiasError(ValueError):
    pass


# 2. Función principal del módulo[cite: 10, 11]
def prestar(catalogo, titulo, dias):
    """
    Gestiona el préstamo de un libro. No imprime nada por pantalla;
    valida rangos y traduce o lanza las excepciones correspondientes.[cite: 10, 11]
    """
    if not (1 <= dias <= 30):
        raise DiasError(dias)
    
    try:
        ejemplares = catalogo[titulo]
    except KeyError:
        raise LibroDesconocidoError(f"no existe {titulo!r}")
        
    if ejemplares <= 0:
        raise SinEjemplaresError(titulo)
        
    catalogo[titulo] -= 1
    return catalogo[titulo]