
"""""
Pide un número entero al usuario y muestra su tabla de multiplicar del 1 al 10, 
utilizando un bucle for y la función range().
"""

numero = int(input("Introduce un número: "))

print(f"La tabla de multiplicar del {numero}: ")

for i in range(1, 11):
    resultado = numero * i
    print(f"{numero} x {i} = {resultado}")
