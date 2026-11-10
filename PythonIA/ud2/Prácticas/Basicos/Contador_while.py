
""""
Escribe un programa que pida números al usuario, uno tras otro, hasta que introduzca un 0. 
Al terminar, muestra cuántos números se han introducido (sin contar el 0) y su suma total.
Contador con while
"""

numero = int(input("Introduce un número: "))

contador = 0
suma = 0

while numero != 0:
    contador += 1
    
    suma += numero
    
    numero = int(input("Introduce otro número (0 para salir): "))

print(f"Has introducido {contador} numeros.")
print(f"La suma total es {suma}")