import sys

edad = int(input("Introduce tu edad"))

carnet_joven = input("¿Tienes el carnet joven? (si/no): ").lower()

print(f"¿Es mayor de edad Y tiene carnet joven? {edad >= 18 and carnet_joven == 'si'}")
print(f"¿Es mayor de edad O tiene carnet joven? {edad >= 18 or carnet_joven == 'si'}")