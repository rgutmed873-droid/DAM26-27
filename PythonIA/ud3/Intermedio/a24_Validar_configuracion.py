# Actividad 24

# 1. Generamos el archivo de prueba (faltan 'user' y 'password')
with open("configuracion.txt", "w", encoding="utf-8") as f:
    f.write("host=localhost\n")
    f.write("port=8080\n")
    f.write("debug=true\n")

# 2. Configuración del programa
claves_obligatorias = ["host", "port", "user", "password", "debug"]
claves_encontradas = []

# 3. Leer archivo y extraer las claves reales
try:
    with open("configuracion.txt", "r", encoding="utf-8") as archivo:
        for linea in archivo:
            linea = linea.strip()
            if "=" in linea:
                # split("=", 1) separa solo por el primer igual que encuentre
                clave, valor = linea.split("=", 1) 
                claves_encontradas.append(clave.strip())
except FileNotFoundError:
    print("El archivo de configuración no existe.")

# 4. Comprobación cruzada
print("=== AUDITORÍA DE CONFIGURACIÓN ===")
todas_presentes = True

for clave_requerida in claves_obligatorias:
    if clave_requerida not in claves_encontradas:
        print(f"Aviso: Falta la clave obligatoria '{clave_requerida}'")
        todas_presentes = False

if todas_presentes:
    print("El archivo de configuración está completo.")
else:
    print("El programa continuará, pero revisa los avisos de configuración.")