# fechas.py

def validar_fecha_dd_mm_aaaa(texto):
    """Valida manualmente una fecha en formato DD/MM/AAAA."""
    partes = texto.split("/")
    
    if len(partes) != 3:
        raise ValueError("El formato debe ser exactamente DD/MM/AAAA separados por '/'")
    
    # Si meten letras en vez de números, esto lanzará un ValueError automáticamente
    dia = int(partes[0])
    mes = int(partes[1])
    anyo = int(partes[2])
    
    if not (1 <= mes <= 12):
        raise ValueError("El mes debe estar entre 1 y 12.")
    
    if anyo < 0:
        raise ValueError("El año no puede ser negativo.")
        
    # Lista con los días máximos de cada mes (posición 0 a 11)
    dias_por_mes = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    
    # Comprobamos si el día supera el límite de ese mes concreto
    if not (1 <= dia <= dias_por_mes[mes - 1]):
        raise ValueError(f"Día incorrecto. El mes {mes} tiene hasta {dias_por_mes[mes-1]} días.")
        
    return True