
numero = int(input("¿Cuantos números pares quieres sumar:? "))

suma = 0

for par in range(2, 2*numero+1, 2):
    suma += par

print(f"La suma de los primeros {numero} números pares es: {suma}")