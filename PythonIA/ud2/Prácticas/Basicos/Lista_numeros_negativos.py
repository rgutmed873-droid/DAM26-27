
"""
Dada una lista de números fija en tu código (por ejemplo, numeros = [4, 9, 2, 3, 8, -7]), 
recorre la lista con un bucle for y utiliza la instrucción break para detener la ejecución en cuanto encuentres el primer número negativo. 
Muestra en pantalla tanto el valor negativo encontrado como su índice/posición. Si la lista no contiene ningún número negativo, 
muéstralo con un mensaje informativo
"""

numeros = [4, 9, 2, 3, 8, -7]
encontrado = False

for i in range(len(numeros)):
    valor = numeros[i]

    if valor < 0:
        print(f"El primer número negativo es: {valor} y su posicion es {i}")
        
        encontrado = True
        break
if not encontrado:
    print("No hay ningún número negativo en la lista")    
