import sys

print("--- FICHA DE ALUMNO ---")

nombre = input("Introduce tu nombre: ")
edad = float(input("Introduce tu edad: "))
nota_media = float(input("Introduce tu nota media: "))

print(f"El nombre del alumno es: {nombre}")
print(f"La edad del alumno es: {edad}")
print(f"La nota media anterior es: {nota_media: .2f}")
