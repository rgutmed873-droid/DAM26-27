# Actividad 22

# 1. Creamos el archivo de pruebas con datos corruptos a propósito
with open("ventas.txt", "w", encoding="utf-8") as f:
    f.write("manzanas,5,1.20\n")
    f.write("peras,2\n")              # Va a fallar porque le falta el campo del precio
    f.write("platanos,10,0.80\n")
    f.write("naranjas,tres,1.50\n")   # Va a fallar porque tres es texto

# 2. Leemos y procesamos descartando errores
print("=== PROCESADOR DE VENTAS ===")
with open("ventas.txt", "r", encoding="utf-8") as f:
    for num_linea, linea_raw in enumerate(f, start=1):
        linea = linea_raw.strip()
        try:
            campos = linea.split(",")
            if len(campos) != 3:
                raise ValueError("No tiene exactamente 3 campos")
            
            producto = campos[0]
            cantidad = int(campos[1])  # Si no es entero, lanza ValueError
            precio = float(campos[2])  # Si no es decimal, lanza ValueError
            
            print(f"Línea {num_linea} OK: {cantidad}x {producto} a {precio}€")
            
        except ValueError as error:
            print(f"Línea {num_linea} descartada ({linea!r}): {error}")