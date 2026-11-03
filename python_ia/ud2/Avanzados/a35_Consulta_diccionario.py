
alumno = {"nombre": "Ana", "edad": 18, "nota": 8.5}

clave = input("Qué dato quieres consutar ('nombre, edad o nota'): ")
print(alumno.get(clave, "Dato no disponible"))
