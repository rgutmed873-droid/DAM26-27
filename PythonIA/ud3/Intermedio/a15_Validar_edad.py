#Actividad 15

#Primero creo la función de comprobar la edad
def validar_edad(edad):
    if edad < 0 or edad > 120:
        raise ValueError("La edad introducida es incorrecta")


#Ahora hago lo que sería el programa principal
try:
    edad_usuario = int(input("Introduce tu edad: "))

    #Llamo a la función de validar la edad
    validar_edad(edad_usuario)

    print(f"Edad correcta registra: {edad_usuario} años.")
except ValueError as error:

    print(f"Error de validación: {error}")