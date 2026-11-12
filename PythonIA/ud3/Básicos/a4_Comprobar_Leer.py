
#Actividad 4

import os

nombre_archivo = "notas.txt"

#Comprobar si el archivo existe
if os.path.exists(nombre_archivo):
    with open(nombre_archivo, "r", encoding="utf-8") as archivo:
        print(f"{nombre_archivo}")
else:
#Si es false, mostramos el aviso
    print(f"El archivo {nombre_archivo} aún no se ha creado")