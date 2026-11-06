
#Actividad 3

#Inicio el contador antes de hacer nada
total_lineas = 0

#Abro el archivo
with open("poema.txt", "r", encoding="utf-8") as archivo:

    #Bucle for para dar una vuelta por todas las lineas
    for linea in archivo:
        total_lineas += 1

#Muestro el resultado por pantalla
print(f"El archivo contiene un total de líneas de: {total_lineas}")        