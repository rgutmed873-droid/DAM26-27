#Actividad 11

#Abro los archivos a la vez
with open("texto1.txt", "r", encoding="utf-8") as f1, open("texto2.txt", "r", encoding="utf-8") as f2:
    #Leo todas las lineas de ambos archivos
    linea1 = f1.readlines()
    linea2 = f2.readlines()

#Hago una funcion en la que compruebo que sean igual
if linea1 == linea2:
    print("Los dos archivos son iguales")
else:
    #Voy a usar el min() para que si un archivo tiene menos contenido que el otro no se salga
    limite = min(len(linea1), len(linea2))

    for i in range(limite):
        if linea1[i] != linea2[i]:
            print(i+1)
            break

