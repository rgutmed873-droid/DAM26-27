#Actividad 40

# Explicación (Punto 4 del enunciado):
# No serviría 'from herramientas.diario import RUTA' porque importaría una copia local del valor primitivo (str).
# Al hacer 'RUTA = "clase.txt"' solo modificarías la variable local de principal.py, mientras que diario.py seguiría usando su propia variable.

import os
from herramientas import diario
from herramientas.diario import EntradaError

# 1. Limpieza previa de clase.txt si existía
if os.path.exists("clase.txt"):
    os.remove("clase.txt")

# 2. Redirección de la variable de módulo
diario.RUTA = "clase.txt"

entradas = [
    "Repaso de with", 
    "", 
    "Excepciones propias y jerarquías de errores", 
    "Paquetes"
]

# 3. Procesamiento de entradas capturando la excepción propia
for entrada in entradas:
    try:
        diario.apuntar(entrada)
    except EntradaError as error:
        print(f"Error capturado: {error}")

# 4. Muestra del contenido final y comprobaciones
print("\n=== CONTENIDO FINAL DE clase.txt ===")
if os.path.exists("clase.txt"):
    with open("clase.txt", "r", encoding="utf-8") as archivo:
        print(archivo.read())

print("¿Se ha creado diario.txt por error?:", os.path.exists("diario.txt"))