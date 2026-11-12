#Actividad 17


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
    #El Valueerror lo declaro por si el usuario en vez de añadir un número añade texto o simbolo
    #El Zerodivision es que no se puede divir el número entre 0
    print(f"No se ha podido calcular: {error}")