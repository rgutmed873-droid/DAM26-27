#Actividad 5

try:
    #Pedimos los números al usuario
    num1 = float(input("Introduce el primer numero: "))
    num2 = float(input("Introduce el segundo numero: "))

    #Hacemos la operación que en este caso es la divison
    resultado = num1 / num2
    print(f"El resultado de la división es: {resultado}")

except ValueError:
    #Imprimo el error de si el valor introducido es distinto de un número
    print("El número introducido no es válido")

except ZeroDivisionError:
    #Imprimo el erro de que si el divisor es 0 no se pueda dividir
    print(f"El divisor {num2} no puede ser 0")
