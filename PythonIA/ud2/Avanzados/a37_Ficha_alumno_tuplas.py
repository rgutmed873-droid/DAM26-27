
alumno = {"nombre": "Lucía", "notas": (7, 8.5, 6)}

suma = 0
for nota in alumno["notas"]:
    suma += nota

media = suma / len(alumno["notas"])
print(f"La media de {alumno['nombre']} es {media:.2f}")