#Actividad 14

#Primero de todo saber el número de intentos al contar las notas
try:
    with open("historial.txt", "r", encoding="utf-8") as archivo:
        intento = len(archivo.readlines()) +1
except FileNotFoundError:
    intento = 1

#Pedir la nota al usuario
nota = float(input("Introduce tu nota: "))

#Guardar el archivo con "a"
with open("historial.txt", "a", encoding="utf-8") as archivo:
    archivo.write(f"Intento {intento}: {nota}\n")

print("La nota se ha registrado correctamente en el archivo historial.txt")    