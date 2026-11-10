import sys

kilometros = float(input("Introduce la distancia en kilómetros: "))

metros = kilometros * 1000
milla = kilometros * 0.621371
milla_nauticas = kilometros * 0.539957

print(f"{kilometros} km equivalen a {metros: .3f} metros.")
print(f"{kilometros} km equivalen a {milla: .3f} millas")
print(f"{kilometros} km equivalen a {milla_nauticas: .3f} millas náuticas")
