import random

numeros = []
for i in range(6):
    numero_aleatorio = random.randint(1, 100)
    numeros.append(numero_aleatorio)

print(numeros)

mayor = numeros[0]
for numero in numeros:
    if numero > mayor:
        mayor = numero

print(f"El número mayor es {mayor}")