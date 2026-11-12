
#Actividad 1
# 1. Pedimos el nombre al usuario
nombre = input("Por favor, introduce tu nombre: ")

# 2. Abrimos (o creamos) el archivo saludo.txt en modo escritura ("w")
with open("saludo.txt", "w", encoding="utf-8") as archivo:

    #3. Llamo al nombre que le he dado al archivo y añado el texto que quiera
    archivo.write(f"Hola {nombre} gracias :)")
    