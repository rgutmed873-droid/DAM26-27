#Funcion

"""
Define una función llamada comprobar_aprobado que reciba un único parámetro llamado nota.

Dentro de la función, utiliza un if/else:

Si la nota es mayor o igual a 5, la función debe devolver (return) el texto "Aprobado".

Si es menor, debe devolver el texto "Suspenso".

Fuera de la función (sin sangría), llama a tu función pasándole un 7 y guarda el resultado en una variable. Luego imprímela.
"""

def comprobar_aprobado(nota):
    if nota >= 5:
        return "Aprobado"
    else:
        return "Suspenso"


resultado = comprobar_aprobado(7)

print(f"Mi resultado de la nota es: {resultado}")
