#Actividad 9

#Abrimo el archivo 

with open("alumnos.txt", "r", encoding="utf-8") as archivo:
    #Recorro el archivo linea a linea
    for linea in archivo:
        #Limpio el \n y divido por comas
        nombre, nota = linea.strip().split(",")
        print(f"El alumno {nombre} tiene una nota de {nota}")

