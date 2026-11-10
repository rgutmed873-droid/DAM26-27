#Actividad 8

#Abrimos el archivo origen en modo lectura
#También abrimos el archivo destion en modo escritura al mismo tiempo

with open("diario.txt", "r", encoding="utf-8") as origen, open("diario_copia.txt", "w", encoding="utf-8") as destino:

#Recorro con un for el archivo origen
    for linea in origen:
        destino.write(linea)

#Imprimo el mensaje de que se ha duplicado
print("El archivo se ha duplicado exitosamente en diario_copia.txt")