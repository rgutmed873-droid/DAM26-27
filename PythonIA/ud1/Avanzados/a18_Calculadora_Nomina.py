import sys

print("CALCULADORA DE LA NOMINA MENSUAL")

salario_brutoAnual = float(input("Introduce tu salario bruto Anual: "))

salario_BrutoMensual = salario_brutoAnual /12

irpf = salario_BrutoMensual * 0.15
ss = salario_BrutoMensual * 0.0635

salario_Neto = salario_BrutoMensual - irpf - ss

total_Nomina = print(f"El salario neto notal es: {salario_Neto: .2f}")

print(f"El salario bruto anual es: {salario_brutoAnual}")
print(f"El salario bruto mensual es: {salario_BrutoMensual}")
print(f"La cantidad de IRPF es de: {irpf}")
print(f"La cantidad de Seguridad Social es: {ss}")

