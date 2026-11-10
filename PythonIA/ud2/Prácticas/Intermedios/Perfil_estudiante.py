#Perfil de estudiante

"""
Enunciado:

Crea un diccionario llamado alumno que contenga tres pares de clave-valor iniciales: "nombre": "Ana", "edad": 20 y "curso": "Python".

Modifica el valor de la clave "edad" para que ahora sea 21.

Añade una nueva clave llamada "nota_final" y asígnale el valor numérico 8.5.

Utiliza un bucle for junto con el método .items() para recorrer todo el diccionario. 
En cada vuelta del bucle, muestra por pantalla un mensaje con este formato:
"""

alumno = {"nombre": "Ana", "edad": 20,  "curso": "Python"}

alumno["edad"] = 21

alumno["nota_final"] = 8.5

print(f"Los datos del alumno son: {alumno}\n")

for clave, valor in alumno.items():
    
    print(f"La clave es {clave} y su valor es {valor}")

