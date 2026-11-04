
contador = 0

suma = 0

numero = int(input("Introduce un número: "))

while numero != 0:
    suma += numero
    contador += 1
    numero = int(input("Introduce otro número (0 para salir): "))

print(f"Has introducido {contador} números (sin contar el 0)")
print(f"La suma total de los números es: {suma}")