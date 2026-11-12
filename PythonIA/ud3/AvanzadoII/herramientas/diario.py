#diario

# herramientas/diario.py
import os

# Variables de módulo
RUTA = "diario.txt"
MAXIMO = 30

# Excepción personalizada.
class EntradaError(Exception):
    pass

def vaciar():
    """Deja vacío el archivo especificado en la variable RUTA."""
    with open(RUTA, "w", encoding="utf-8") as archivo:
        pass

def apuntar(texto):
    """
    Limpia los espacios de texto y añade la línea numerada a RUTA.
    Lanza EntradaError si el texto está vacío o supera MAXIMO caracteres.
    """
    texto_limpio = texto.strip()
    
    if not texto_limpio or len(texto_limpio) > MAXIMO:
        raise EntradaError(f"entrada no válida: {texto!r}")
        
    num_lineas = 0
    try:
        with open(RUTA, "r", encoding="utf-8") as archivo:
            num_lineas = len(archivo.readlines())
    except FileNotFoundError:
        pass  # El modo 'a' creará el archivo si no existe

    siguiente_numero = num_lineas + 1
    with open(RUTA, "a", encoding="utf-8") as archivo:
        archivo.write(f"{siguiente_numero:02d}. {texto_limpio}\n")


# Autotest: Solo se ejecuta si corres este archivo directamente
if __name__ == "__main__":
    RUTA = "prueba_diario.txt"
    vaciar()
    apuntar("uno")
    apuntar("dos")
    
    with open(RUTA, "r", encoding="utf-8") as archivo:
        lineas = archivo.readlines()
        
    assert lineas == ["01. uno\n", "02. dos\n"], f"Error en autotest: {lineas}"
    os.remove(RUTA)
    print("diario.py: autotest superado")