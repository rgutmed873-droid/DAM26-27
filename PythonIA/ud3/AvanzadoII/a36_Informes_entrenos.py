#Actividad 36 que tendrá el main del ejercicio


# --- FUNCIONES DE CONVERSIÓN ---
def a_segundos(tiempo):
    """Convierte un texto 'm:ss' a segundos totales."""
    minutos, segundos = tiempo.split(":")
    return int(minutos) * 60 + int(segundos)

def formatear(segundos_totales):
    """Convierte segundos totales a un texto 'm:ss'."""
    minutos = segundos_totales // 60
    segundos = segundos_totales % 60
    return f"{minutos}:{segundos:02d}" # :02d obliga a que los segundos tengan 2 cifras


# --- PROGRAMA PRINCIPAL ---
tiempos_acumulados = {}
km_acumulados = {}

# Abrimos los dos archivos a la vez en un único with
with open("entrenos.csv", "r", encoding="utf-8") as f_lectura, open("descartes.txt", "w", encoding="utf-8") as f_descartes:
    
    # Nos saltamos la primera línea (cabecera)
    f_lectura.readline()
    
    # Leemos el resto enumerando desde la línea 2
    for n, linea_original in enumerate(f_lectura, start=2):
        linea = linea_original.strip()
        
        # PRIMER TRY: Comprobar el formato (separación por comas)
        try:
            fecha, deportista, tiempo, km = linea.split(",")
        except ValueError:
            f_descartes.write(f"Línea {n}: formato incorrecto {linea!r}\n")
            continue # Saltamos a la siguiente vuelta del bucle
            
        # Limpiamos el nombre por si trae espacios del CSV (ej: " Ana")
        deportista = deportista.strip()
        
        # SEGUNDO TRY: Comprobar que los valores matemáticos son correctos
        try:
            segs = a_segundos(tiempo)
            distancia = float(km)
        except ValueError:
            f_descartes.write(f"Línea {n}: valor no válido {linea!r}\n")
            continue
            
        # Si llega aquí, la línea es perfecta. Acumulamos en los diccionarios usando .get()
        tiempos_acumulados[deportista] = tiempos_acumulados.get(deportista, 0) + segs
        km_acumulados[deportista] = km_acumulados.get(deportista, 0) + distancia

# --- MOSTRAR RESULTADOS ---
print("=== INFORME DE ENTRENAMIENTOS ===")
objetivo = 10.0

for deportista in tiempos_acumulados:
    tiempo_formateado = formatear(tiempos_acumulados[deportista])
    km_totales = km_acumulados[deportista]
    diferencia = km_totales - objetivo
    
    # El :+.1f fuerza a poner el signo (+ o -) y redondear a 1 decimal
    print(f"{deportista}: {tiempo_formateado} en {km_totales} km ({diferencia:+.1f} km respecto al objetivo)")

# --- MOSTRAR DESCARTES ---
# Volvemos a abrir el archivo de descartes para contar cuántas líneas tiene
with open("descartes.txt", "r", encoding="utf-8") as f_descartes:
    cantidad_errores = len(f_descartes.readlines())
    print(f"\n Se han descartado {cantidad_errores} líneas con errores (revisa descartes.txt).")