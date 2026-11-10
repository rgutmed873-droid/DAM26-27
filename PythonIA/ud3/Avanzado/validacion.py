#validacion.py

# validaciones.py

def validar_nota(nota):
    """Comprueba que la nota esté entre 0 y 10. Si no, lanza ValueError."""
    if nota < 0 or nota > 10:
            raise ValueError("La nota no puede ser mayor de 10 y tampoco inferior de 0")
    