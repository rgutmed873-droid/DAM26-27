#Actividad 17

# Actividad 17: Captura múltiple en un conversor

try:
    # 1. Pedimos los datos al usuario
    num1 = float(input("Introduce el primer número: "))
    num2 = float(input("Introduce el segundo número: "))
    operador = input("Introduce la operación (+, -, *, /): ")

    # 2. Realizamos la operación correspondiente
    if operador == "+":
        resultado = num1 + num2
    elif operador == "-":
        resultado = num1 - num2
    elif operador == "*":
        resultado = num1 * num2
    elif operador == "/":
        resultado = num1 / num2
    else:
        resultado = "Operador no válido"

    print(f"Resultado: {resultado}")

except (ValueError, ZeroDivisionError) as error:
    print(f"No se ha podido calcular: {error}")