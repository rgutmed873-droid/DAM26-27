#Actividad 16

try:
    #Voy a intentar abrir un archivo que no existe
    with open("inventario.txt", "r", encoding="utf-8") as archivo:
        contenido = archivo.read()
        print("Contenido del inventario")
        print(contenido)
except FileNotFoundError:
    #Si salta un error le muestro un mensaje al usuario
    print("El archivo 'inventario.txt' no existía. Se ha creado uno nuevo")
    with open("inventario.txt", "w", encoding="utf-8") as archivo:
        pass