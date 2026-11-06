#Actividad 12

#Primero de todo abro el archivo
with open("diario.txt", "r", encoding="utf-8") as origen, open("diario_mayus.txt", "w", encoding="utf-8") as destino:

    #Recorro el archivo linea a linea
    for linea in origen:
        destino.write(linea.upper())

#Muestro que se ha realizado el cambio
print("La copia se ha hecho en mayúscula")