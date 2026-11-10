#Actividad 38

# ==========================================
# PROGRAMA 1: finally con return, continue y break[cite: 9]
# ==========================================
print("--- PROGRAMA 1 ---")

def mitad(texto):
    try:
        n = int(texto)
        return n // 2
    except ValueError:
        return -1
    finally:
        print(f"finally({texto})")

for texto in ["8", "", "x", "-4", "6"]:
    try:
        if texto == "":
            continue
        r = mitad(texto)
        if r < 0:
            break
    except ValueError:
        print("except")
    else:
        print(f"else {r}")
    finally:
        print("fin de vuelta")
        
print("después del bucle")


# ==========================================
# PROGRAMA 2: propagación y un try que no captura ese tipo[cite: 10]
# ==========================================
print("\n--- PROGRAMA 2 ---")

def leer(precios, producto):
    return precios[producto]

def informe(precios, producto):
    try:
        precio = leer(precios, producto)
        if precio < 0:
            raise ValueError("precio negativo")
    except ValueError as error:
        print(f"{producto}: {error}")
    else:
        print(f"{producto}: {precio} €")

precios = {"pan": 1.2, "sal": -1}

try:
    informe(precios, "pan")
    informe(precios, "sal")
    informe(precios, "té")
    informe(precios, "pan") # ¿Llegará a ejecutarse esta línea?
except KeyError as error:
    print(f"Capturado fuera: {type(error).__name__} {error}")

print("\n--- PROGRAMA 3 ---")
# Error en la captura múltiple
datos = [10, 0, "3"]
for i in range(4):
    try:
        print(100 / datos[i])
    except (ZeroDivisionError, TypeError):
        print("división imposible")
    except Exception as error:
        print(f"inesperado: {type(error).__name__}")


print("\n--- PROGRAMA 4 ---")
# Error en el cierre del archivo
def contar_lineas(ruta):
    with open(ruta, "r", encoding="utf-8") as archivo:
        return len(archivo.readline())
      

try:
    print(contar_lineas("no_existe.txt"))
except FileNotFoundError:
    print("El archivo no existe")
except Exception as error:
    print(f"Otro error: {type(error).__name__}")


print("\n--- PROGRAMA 5 ---")
# raise a secas, error dentro de un except y try anidados

def a_numero(texto):
    try:
        return int(texto)
    except ValueError:
        print(f" aviso: {texto!r} no es entero")
        raise

for texto in ["4", "2.5", "dos"]:
    try:
        try:
            n = a_numero(texto)
        except ValueError:
            n = int(float(texto))
            print(f"{texto}: truncado a {n}")
        else:
            print(f"{texto}: entero {n}")
    except ValueError as error:
        print(f"{texto}: {type(error).__name__}")


print("\n--- PROGRAMA 6 ---")
# Excepciones habituales y la variable 'as error' fuera del except

notas = {"Ana": [7, 9], "Luis": []}
for nombre in ["Ana", "Luis", "Eva"]:
    try:
        try:
            lista = notas[nombre]
            # Sumamos las notas y calculamos la media
            print(nombre, lista[1], sum(lista) / len(lista))
        except (KeyError, IndexError) as error:
            print(f"{nombre}: {type(error).__name__} {error}")
            
        print("Total: " + 16)
        
    except TypeError as error:
        mensaje = str(error)
        
    # Imprimimos fuera del bloque except
    print(mensaje)
    #print(error)