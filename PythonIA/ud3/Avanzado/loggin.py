# logger.py

def registrar(nivel, mensaje):
    """Guarda un mensaje en el log con su nivel (INFO, ERROR, etc.)"""
    # Uso el upper para que aparezca siempre en mayúscula
    etiqueta = f"[{nivel.upper()}]"
    
    # Abrimos en modo "a" (append) para añadir al final sin borrar lo anterior
    with open("aplicacion.log", "a", encoding="utf-8") as archivo:
        archivo.write(f"{etiqueta} {mensaje}\n")