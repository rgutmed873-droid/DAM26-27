import sys

edad_texto = input("Introduce tu edad: ") 
edad = int(edad_texto) 
print(f"Dentro de 5 años tendrás {edad + 5} años") 

#Me da el error de ValueError porque está intentando convertir
#un tipo texto a int pero no tiene la forma válida de hacerlo