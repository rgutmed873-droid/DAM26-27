#Actividad 7

#Pedir al usuario el texto que queremos guardar
texto = input("Escribe el texto que quieras: ")

#Abrimos el archivo con a para añadir
with open("diario.txt", "a", encoding="utf-8") as archivo:
    archivo.write(texto +"\n")

print("La entrada en el diario se ha guardado en 'diario.txt'")