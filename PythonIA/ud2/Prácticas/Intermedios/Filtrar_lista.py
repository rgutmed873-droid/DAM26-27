
#Filtrar una lista
"""
Tienes una lista inicial en tu código: numeros = [12, 7, 45, 3, 89, 21, 6].

Crea una nueva lista vacía (por ejemplo, filtrados = []).

Recorre la lista numeros usando un bucle for.

Dentro del bucle, utiliza un if para comprobar si el número de esa vuelta es mayor que 10.

Si cumple la condición, añádelo a la nueva lista utilizando el método que vimos en el glosario: .append().

Fuera del bucle, muestra por pantalla tanto la lista original como la lista nueva ya filtrada.
"""

numeros = [12, 7, 45, 3, 89, 21, 6]

filtrado = []

for numero in numeros:
    if numero > 10:
        filtrado.append(numero)

print(f"La lista original: {numeros}")
print(f"La lista filtrada y modificada: {filtrado}")