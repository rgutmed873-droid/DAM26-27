import sys

x1 = float(input("Introduce la coordenada x1: "))
y1 = float(input("Introduce la coordenada y1: "))
x2 = float(input("Introudce la coordenada x2: "))
y2 = float(input("Introduce la coordenada y2: "))

distancia = ((x2-x1)**2 + (y2-y1)**2) ** 0.5
es_mayor = distancia > 10

print(f"La distancia total es: {distancia: .3f}")
print(f"¿La distancia es mayor de 10? {es_mayor}")