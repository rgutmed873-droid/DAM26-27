
def calcular(n1, n2, operador):
    if operador == "+":
        return n1 + n2
    elif operador == "-":
        return n1 - n2
    elif operador == "*":
        return n1 * n2
    elif operador == "/":
        if n2 == 0:
            return "No se puede dividir entre 0"
        else:
            return n1 / n2
    else:
        return "Operador no válido"


numero1 = float(input("Primer número: "))
numero2 = float(input("Segundo número: "))
signo = input("Operador (+, -, *, /): ")

resultado = calcular(numero1, numero2, signo)
print(f"Resultado: {resultado}")
