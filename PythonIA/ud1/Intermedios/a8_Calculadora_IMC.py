import sys

peso = float(input("Introduce tu peso en Kilogramos"))
altura = float(input("Introduce tu peso en Metros"))

imc = peso / altura **2

print(f"Tu IMC es: {imc: .2f}")

print(f"¿El IMC es menor que 25? {imc < 25}")
print(f"¿El IMC es igual a 25? {imc == 25}")
print(f"¿El IMC es mayor que 25? {imc > 25}")
