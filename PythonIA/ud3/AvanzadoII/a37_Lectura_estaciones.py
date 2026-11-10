#Actividad 37


# 1. Creamos el archivo lecturas.ini de prueba con sus defectos a propósito
contenido = """# Lecturas de temperatura
08:00 14.5
[Norte]
08:00 12.5
08:10 =
08:20=13.0
[Sur]
08:00=-
08:10=18.25
09:00=19
revisado"""

with open("lecturas.ini", "w", encoding="utf-8") as archivo:
    archivo.write(contenido)


# 2. Función principal de lectura y procesamiento
def leer_lecturas(ruta):
    """
    Lee un archivo de lecturas por secciones, procesa comentarios, imputa
    valores faltantes y devuelve la tupla (datos, errores).
    """
    datos = {}
    errores = []
    
    seccion_actual = None
    ultimo_valor_valido = None

    with open(ruta, "r", encoding="utf-8") as archivo:
        for num_linea, linea_raw in enumerate(archivo, start=1):
            # Limpiamos comentarios post-línea (;) e ignoramos espacios externos
            linea = linea_raw.split(";")[0].strip()
            
            # Ignoramos líneas vacías o que son comentarios (#)[cite: 10]
            if not linea or linea.startswith("#"):
                continue

            # Detección de secciones: [Nombre][cite: 10]
            if linea.startswith("[") and linea.endswith("]"):
                seccion_actual = linea[1:-1].strip()
                if seccion_actual not in datos:
                    datos[seccion_actual] = {}
                ultimo_valor_valido = None  # Reseteamos el último valor para la nueva sección[cite: 10]
                continue

            # Validación 1: Comprobar si falta el signo '='[cite: 10]
            if "=" not in linea:
                errores.append(f"Línea {num_linea}: falta '=' en {linea!r}")
                continue

            # Validación 2: Comprobar si hay un dato antes de abrir una sección[cite: 10]
            if seccion_actual is None:
                errores.append(f"Línea {num_linea}: dato fuera de sección")
                continue

            # Separamos clave (hora) y valor[cite: 10]
            hora_str, valor_str = linea.split("=", 1)
            hora = hora_str.strip()
            val_limpio = valor_str.strip()

            # Conversión de valor con imputación de datos si falla el float()[cite: 10]
            try:
                valor = float(val_limpio)
                ultimo_valor_valido = valor  # Guardamos como último valor válido
            except ValueError:
                # Si falla la conversión (vacío, '-', etc.), intentamos imputar[cite: 10]
                if ultimo_valor_valido is not None:
                    valor = ultimo_valor_valido
                else:
                    errores.append(f"Línea {num_linea}: {val_limpio!r} sin valor anterior")
                    continue
            
            # Guardamos el dato procesado en la sección correspondiente
            datos[seccion_actual][hora] = valor

    return datos, errores


# 3. PROGRAMA PRINCIPAL[cite: 10]
datos, errores = leer_lecturas("lecturas.ini")

print("=== DICCIONARIO DE DATOS PROCESADOS ===")
print(datos)

print("\n=== LISTADO DE ERRORES REGISTRADOS ===")
for error in errores:
    print(error)

print("\n=== CONSULTA ESPECÍFICA ===")
if "Norte" in datos and "08:10" in datos["Norte"]:
    print(f"Valor de datos['Norte']['08:10']: {datos['Norte']['08:10']}")
else:
    print("La clave datos['Norte']['08:10'] no existe debido a un error de origen.")