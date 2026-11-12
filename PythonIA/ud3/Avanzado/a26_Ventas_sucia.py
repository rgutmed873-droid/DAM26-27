# Actividad 26

# 1. Creamos el archivo de pruebas con datos buenos y "sucios"
with open("ventas.txt", "w", encoding="utf-8") as f:
    f.write("teclado,2,15.50\n")
    f.write("raton,1\n")            # ERROR: Falta una coma y el precio
    f.write("monitor,1,120.00\n")
    f.write("cable,dos,5.00\n")     # ERROR: 'dos' es texto, no un número
    f.write("altavoces,3,20.00\n")

# 2. Procesamiento del archivo
total_facturado = 0.0

print("=== REGISTRO DE FACTURACIÓN ===")

with open("ventas.txt", "r", encoding="utf-8") as archivo:
    for linea_raw in archivo:
        linea = linea_raw.strip()
        
        # Ignoramos líneas que estén totalmente en blanco
        if not linea:
            continue
            
        try:
            # Si no tiene los 3 datos que se requiere lanza el ValueError
            producto, cantidad_texto, precio_texto = linea.split(",")
            
            # Si no son números se lanza el ValueError
            cantidad = int(cantidad_texto)
            precio = float(precio_texto)
            
            # Si todo va bien se ejecuta
            subtotal = cantidad * precio
            total_facturado += subtotal
            
        except ValueError:
            # Capturamos cualquier error de la línea y avisamos sin parar el programa
            print(f"Línea descartada por formato incorrecto: {linea!r}")

print("-" * 30)
print(f"Total facturado válido: {total_facturado:.2f} €")