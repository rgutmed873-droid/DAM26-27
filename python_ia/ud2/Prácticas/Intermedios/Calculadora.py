""""
Actividad 17: Calculadora con operador
"""

# Definir la función (Solo recibe datos, calcula y devuelve)
def calcular(n1,n2,operador):
    if operador == "+":
        return n1 + n2
    elif operador == "-":
        return n1 - n2
    elif operador == "*":
        return n1 * n2
    elif operador == "/":
        if n2 == 0:
            return "Error: no se puede dividir entre 0"
        else:
            return n1 / n2
    else:
         return "Operador no reconocido"

#Programa principal para pedir datos e interacción con el usuario

numero1 = int(input("Introduce el primer número: "))
numero2 = int(input("Introduce el segundo número: "))
operador = input("Introduce un signo de operacion (+,-,*,/): ")

#Llamamos a la función
resultado = calcular(numero1,numero2,operador)

#Mostramos por la pantalla
print(f"El resultado de {numero1} {operador} {numero2} es: {resultado}")
