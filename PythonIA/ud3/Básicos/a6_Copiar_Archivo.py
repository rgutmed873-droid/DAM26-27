#Actividad 6

try:
    #Primero intento abrir y leer el archivo
    with open("agenda.txt", "r", encoding="utf-8") as archivo:
        contenido = archivo.read()
        print("--- Contenido de la agenda ---")
        print(contenido)

except FileNotFoundError:
    #Aquí muestro el error en el que no puede leer el archivo de agenda o no lo encuentra
    print("El archivo agenda.txt no se encuentra")        