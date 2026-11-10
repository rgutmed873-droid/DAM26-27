#Actividad 19

suma_temperaturas = 0.0
contador_validas = 0


with open("temperaturas.txt", "r", encoding="utf-8") as archivo:
    for linea in archivo:
        try:
            temp = float(linea.strip())
            
            suma_temperaturas += temp
            contador_validas += 1
            
        except ValueError:
            # Si la línea no es un número válido, mostramos un aviso y el bucle continúa
            print(f"Línea ignorada por formato incorrecto: {linea.strip()!r}")

# Calculamos y mostramos la media si hubo al menos un dato válido
if contador_validas > 0:
    media = suma_temperaturas / contador_validas
    print(f"\nMedia de temperaturas válidas ({contador_validas} lecturas): {media:.2f} °C")
else:
    print("No se encontraron temperaturas válidas.")